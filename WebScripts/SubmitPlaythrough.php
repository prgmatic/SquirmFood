<?php
require('Connect.php');

$message = "";
$playthroughID = -1;
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $testerName    = StripInput($_POST["TesterName"]);
    $sceneGUID     = StripInput($_POST["SceneGUID"]);
    $layoutGUID    = StripInput($_POST["LayoutGUID"]);
    $totalMoves    = StripInput($_POST["TotalMoves"]);
    $movesOnWin    = StripInput($_POST["MovesOnWin"]);
    $difficulty    = StripInput($_POST["Difficulty"]);
    $satisfaction  = StripInput($_POST["Satisfaction"]);
    $notes         = StripInput($_POST["Notes"]);
    $duration      = StripInput($_POST["Duration"]);
    $actions       = StripInput($_POST["Actions"]);
    
    $message .= "$testerName
$sceneGUID
$layoutGUID
$totalMoves
$movesOnWin
$difficulty
$satisfaction
$notes
$duration
$actions\r\n";
    
    $time = time();
    $query = "INSERT INTO Playthroughs (TesterName, SceneGUID, LayoutGUID, TotalMoves, MovesOnWin, Difficulty, Satisfaction, Notes, Duration, Time, Actions)
    VALUES('$testerName', '$sceneGUID', '$layoutGUID', $totalMoves, $movesOnWin, $difficulty, $satisfaction, '$notes', $duration, $time, '$actions')";
    
    if($conn->query($query))
    {
        $playthroughID = $conn->insert_id;
        $message .= "Entry succesfully added to database...\r\n";
    }
    else
        $message .= "Error: " . mysql_error() . "\r\n";
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'PlaythroughID' => $playthroughID
    );
echo json_encode($response);