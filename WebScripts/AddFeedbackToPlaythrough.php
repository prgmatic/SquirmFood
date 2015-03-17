<?php
require('Connect.php');

$message = "";
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $id                = StripInput($_POST["PlaythroughID"]);
    $testerKey         = StripInput($_POST["TesterKey"]);
    $difficulty        = StripInput($_POST["Difficulty"]);
    $satisfaction      = StripInput($_POST["Satisfaction"]);
    $notes             = StripInput($_POST["Notes"]);
    
    if($db->AddFeedbackToPlaythrough($testerKey, $id, $difficulty, $satisfaction, $notes))
        $message .= "Feedback Updated...\r\n";
    else
        $message .= $db->GetError();
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message
);
echo json_encode($response);
?>