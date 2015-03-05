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
    
    $ip = GetClientIP();
    $time = time();
    
    $message .= "$testerName
$ip
$sceneGUID
$layoutGUID
$totalMoves
$movesOnWin
$difficulty
$satisfaction
$notes
$duration
$actions\r\n";
    
    $query = "INSERT INTO Playthroughs (TesterName, IP, SceneGUID, LayoutGUID, TotalMoves, MovesOnWin, Difficulty, Satisfaction, Notes, Duration, Time, Actions)
    VALUES('$testerName', '$ip', '$sceneGUID', '$layoutGUID', $totalMoves, $movesOnWin, $difficulty, $satisfaction, '$notes', $duration, $time, '$actions')";
    
    if($conn->query($query))
    {
        $playthroughID = $conn->insert_id;
        $message .= "Entry succesfully added to database...\r\n";
    }
    else
        $message .= "Error: " . $conn->error . "\r\n";
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'PlaythroughID' => $playthroughID
    );
echo json_encode($response);


function GetClientIP() {
    $ipaddress = '';
    if (getenv('HTTP_CLIENT_IP'))
        $ipaddress = getenv('HTTP_CLIENT_IP');
    else if(getenv('HTTP_X_FORWARDED_FOR'))
        $ipaddress = getenv('HTTP_X_FORWARDED_FOR');
    else if(getenv('HTTP_X_FORWARDED'))
        $ipaddress = getenv('HTTP_X_FORWARDED');
    else if(getenv('HTTP_FORWARDED_FOR'))
        $ipaddress = getenv('HTTP_FORWARDED_FOR');
    else if(getenv('HTTP_FORWARDED'))
       $ipaddress = getenv('HTTP_FORWARDED');
    else if(getenv('REMOTE_ADDR'))
        $ipaddress = getenv('REMOTE_ADDR');
    else
        $ipaddress = 'UNKNOWN';
    return $ipaddress;
}