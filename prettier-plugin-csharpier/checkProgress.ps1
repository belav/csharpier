$types = Get-ChildItem ./src/Printer/Types
$total = 0
$unfinished = 0;
foreach($file in $types) {
    $total++;

    if((Select-String -Path $file.FullName -Pattern "TODO Node").Length -gt 0 )
    {
        $unfinished++;
    }
}

$text = ($total - $unfinished).ToString() + " of " + $total.ToString()
$percent = ($total - $unfinished) / $total * 100;
Write-Output $text
Write-Output $unfinished
Write-Output $percent

