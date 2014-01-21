function Main {
<#
.SYNOPSIS
Reports current time.

.DESCRIPTION
Copyright (C) 2014 XPFriend Community.
This software is released under the MIT License.
#>
    DATA texts {
        @{Subject = "It's"; Format = "{0}"}
    }
    Import-LocalizedData -BindingVariable texts -ErrorAction:SilentlyContinue

    $newReport = @{}

    $newReport.Title = Get-Date -f HH:mm
    $newReport.Subject = $texts.Subject
    $newReport.CurrentStatus = $texts.Format -f $newReport.Title
    $newReport.IsUpdated = $report.Title -ne $newReport.Title
    if($newReport.Title.EndsWith("00")) {
        $newReport.CurrentGrade = 1
    } else {
        $newReport.CurrentGrade = 0
    }

    $newReport
}
