function Main {
    if($report.CurrentGrade -eq 0) {
        "Normal"
    } elseif($report.CurrentGrade -gt 0) {
        "Happy"
    } elseif($report.CurrentGrade -gt $report.LastGrade) {
        "Expectant"
    } elseif($report.CurrentGrade -lt -2) {
        "Rageful"
    } elseif($report.CurrentGrade -lt -1) {
        "Angry"
    } elseif($report.CurrentGrade -lt 0) {
        "Sad"
    } else {
        "Normal"
    }
}
