<?php
require('Connect.php');

$message = "";
$playthroughID = -1;
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $authorName    = StripInput($_POST["TesterName"]);
    $authorKey     = StripInput($_POST["TesterKey"]);
    $totalMoves    = StripInput($_POST["TotalMoves"]);
    $movesOnWin    = StripInput($_POST["MovesOnWin"]);
    $duration      = StripInput($_POST["Duration"]);
    $actions       = StripInput($_POST["Actions"]);
    
    $levelName     = StripInput($_POST["LevelName"]);
    $levelData     = StripInput($_POST["LevelData"]);
    
    $message .= "$levelID
$testerName
$totalMoves
$movesOnWin
$difficulty
$satisfaction
$notes
$duration
$actions\r\n";
    
    if($db->GetPlayer($authorKey) != null)
    {
        $db->SubmitLevel($levelName, $authorName, $authorKey, $levelData, $totalMoves, $movesOnWin, $duration, $actions);
        //$playthroughID = $db->AddPlaythrough($levelID, $testerName, $testerKey, $totalMoves, $movesOnWin, $difficulty, $satisfaction, $notes, $duration, $actions);
        $message .= $db->GetError() . "\r\n";
    }
    else
        $message .= "Your key is invalid.\r\n";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
);
echo json_encode($response);