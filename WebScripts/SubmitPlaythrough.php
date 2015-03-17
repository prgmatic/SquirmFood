<?php
require('Connect.php');

$message = "";
$playthroughID = -1;
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $levelID       = StripInput($_POST["LevelID"]);
    $testerName    = StripInput($_POST["TesterName"]);
    $testerKey     = StripInput($_POST["TesterKey"]);
    $totalMoves    = StripInput($_POST["TotalMoves"]);
    $movesOnWin    = StripInput($_POST["MovesOnWin"]);
    $difficulty    = StripInput($_POST["Difficulty"]);
    $satisfaction  = StripInput($_POST["Satisfaction"]);
    $notes         = StripInput($_POST["Notes"]);
    $duration      = StripInput($_POST["Duration"]);
    $actions       = StripInput($_POST["Actions"]);
    
    $message .= "$levelID
$testerName
$totalMoves
$movesOnWin
$difficulty
$satisfaction
$notes
$duration
$actions\r\n";
    
    if($db->GetPlayer($testerKey) != null)
    {
        $playthroughID = $db->AddPlaythrough($levelID, $testerName, $testerKey, $totalMoves, $movesOnWin, $difficulty, $satisfaction, $notes, $duration, $actions);
        if($playthroughID > -1)
            $message .= "Your playthrough has been added...\r\n";
        else
            $message .= $db->GetError();
    }
    else
        $message .= "Your key is invalid.\r\n";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'PlaythroughID' => $playthroughID
);
echo json_encode($response);