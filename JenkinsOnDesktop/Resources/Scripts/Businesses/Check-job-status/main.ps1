function Main {
<#
.SYNOPSIS
Reports job statuses of a build server.

.DESCRIPTION
Copyright (C) 2014-2016 XPFriend Community.
This software is released under the MIT License.

.EXAMPLE
Main http://localhost:8080/
(Reports all job statuses at the server "http://localhost:8080/".)

.EXAMPLE
Main http://localhost:8080/ job1,job2
(Reports statuses of "job1" and "job2" at the server "http://localhost:8080/".)

.EXAMPLE
Main http://localhost:8080/ -user abc -password xxx
(If your server needs user id and password, use "-user" and "-password".)
#>
Param(
    [parameter(mandatory=$true)][string]$url,
    [string[]]$jobs,
    [string]$user,
    [string]$password
)

    DATA texts {
        @{
            CannotFindTheJob = "cannot find the job ({0}) @ {1}";
            Failed = "failed";
            Succeeded = "succeeded"
        }
    }

    function ToHashtable($element) {
        $ht = @{}
        $element.Attributes | foreach {$ht[$_.Name] = $_.Value}
        $ht
    }

    function GetJobStatus() {
        $client = New-Object System.Net.WebClient
        try {
            if(![System.String]::IsNullOrEmpty($user)) {
                $authInfo = [System.Convert]::ToBase64String([System.Text.Encoding]::Default.GetBytes($user + ":" + $password))
                $client.Headers.Add("Authorization", "Basic " + $authInfo)
            }
            $document = [xml]$client.DownloadString($url)

            $projects = $document.Projects.Project

            if($jobs -ne $null) {
                $projects = $projects | where name -in $jobs
            }

            $result = @{}
            $projects | foreach {$result[$_.name] = ToHashtable($_)}
            $result
        } finally {
            $client.Dispose()
        }
    }

    function CreateReport() {
        $lastJobStatus = $report.JobStatus

        if($lastJobStatus -eq $null) {
            $lastJobStatus = @{}
        }

        $maxFailureCount = 0
        $totalFailureCount = 0
        $fixedJob = $null
        $brokenJob = $null

        foreach($c in $currentJobStatus.Values) {
            $l = $lastJobStatus[$c.name]
            if($l -eq $null) {
                $l = @{failureCount = 0}
            }

            if(($c.lastBuildLabel -eq $l.lastBuildLabel) -or ($c.activity -eq "Building")) {
                $c.failureCount = $l.failureCount
                $totalFailureCount += $c.failureCount
                continue
            }

            if($c.lastBuildStatus -eq "Failure") {
                $c.failureCount = $l.failureCount + 1
                if($c.failureCount -gt $maxFailureCount) {
                    $maxFailureCount = $c.failureCount
                    $brokenJob = $c
                }
            } elseif($c.lastBuildStatus -eq "Success") {
                $c.failureCount = 0
                if($l.failureCount -gt 0) {
                    $fixed = $true
                    $fixedJob = $c
                }
            } else {
                $c.failureCount = $l.failureCount
            }
            $totalFailureCount += $c.failureCount
        }

        $newReport = $report.Clone()
        $newReport.LastGrade = $report.CurrentGrade
        $newReport.JobStatus = $currentJobStatus
        $newReport.CurrentGrade = -($totalFailureCount / 4)

        $topic = $null
        if($brokenJob -ne $null) {
            $topic = $brokenJob
            $newReport.CurrentStatus = $texts.Failed
        } elseif($fixedJob -ne $null) {
            $topic = $fixedJob
            $newReport.CurrentStatus = $texts.Succeeded
        }

        if($topic -ne $null) {
            $newReport.IsUpdated = $true
            $newReport.Title = $topic.name
            $newReport.Subject = $topic.name
            $newReport.SourceUrl = $topic.webUrl
        } else {
            $newReport.IsUpdated = $false
        }
        $newReport
    }


    $ErrorActionPreference = "Stop"
    Import-LocalizedData -BindingVariable texts -ErrorAction:SilentlyContinue

    if(!$url.EndsWith("/cc.xml")) {
        if(!$url.EndsWith("/")) {
            $url = $url + "/"
        }
        $url = $url + "cc.xml"
    }

    $currentJobStatus = GetJobStatus
    if($currentJobStatus.Count -eq 0) {
        throw $texts.CannotFindTheJob -f [String]::Join(",", $jobs), $url
    }

    CreateReport
}
