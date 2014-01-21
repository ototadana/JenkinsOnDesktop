$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".Tests.", ".")
. "$here\$sut"

function Using-Culture ([System.Globalization.CultureInfo]$culture, [ScriptBlock]$script) {
    $currentThread = [System.Threading.Thread]::CurrentThread
    $currentCulture = $currentThread.CurrentCulture
    try {
        $currentThread.CurrentCulture = $culture
        $currentThread.CurrentUICulture = $culture
        Invoke-Command $script
    } finally {
        $currentThread.CurrentCulture = $currentCulture
        $currentThread.CurrentUICulture = $currentCulture
    }
}

Describe "Main" {
    $timeFormat = "([0-1][0-9]|[2][0-3]):[0-5][0-9]"
    $timeExp = "^" + $timeFormat + "$"

    It "reports current time." {
        Using-Culture "" {
            # when
            $report = Main

            # then
            $report.Title | Should Match $timeExp
            $report.Subject | Should Be "It's"
            $report.CurrentStatus | Should Match $timeExp
        }
    }

    It "can report current time in Japanese." {
        Using-Culture ja-JP {
            # when
            $report = Main

            # then
            $report.Subject | Should Be "時刻は"
            $report.CurrentStatus | Should Match "^" + $timeFormat + "です$"
        }
    }

    Context "When current time is updated," {
        It "reports that the status is updated." {
            # when
            $report = @{Title = "99:99"}
            $newReport = &{$report | Main}

            # then
            $newReport.IsUpdated | Should Be $true
        }
    }

    Context "When current time is not updated," {
        It "reports that the status is not updated." {
            # setup
            Mock Get-Date {"20:20"}
            
            # when
            $report = @{Title = "20:20"}
            $newReport = &{$report | Main}

            # then
            $newReport.IsUpdated | Should Be $false
        }
    }

    Context "If current time is not 0 minutes," {
        It "reports that the grade is 0." {
            # setup
            Mock Get-Date {"20:01"}

            # when
            $report = Main

            # then
            $report.CurrentGrade | Should Be 0
        }
    }

    Context "If current time is 0 minutes," {
        It "reports that the grade is 1." {
            # setup
            Mock Get-Date {"20:00"}

            # when
            $report = Main

            # then
            $report.CurrentGrade | Should Be 1
        }
    }
}
