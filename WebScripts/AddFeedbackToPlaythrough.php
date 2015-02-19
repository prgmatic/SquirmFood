<?php
require('Connect.php');

$actions = "";
$message = "";
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $id                = StripInput($_POST["PlaythroughID"]);
    $difficulty        = StripInput($_POST["Difficulty"]);
    $satisfaction      = StripInput($_POST["Satisfaction"]);
    $notes             = StripInput($_POST["Notes"]);
    

    $query = "UPDATE Playthroughs SET Difficulty = $difficulty, Satisfaction = $satisfaction, Notes = '$notes' WHERE id = $id";
    if($conn->query($query))
    {
        $message .= "Feedback Added...\r\n";
    }
    else
    {
        $message .= "Could not add feedback.\r\n";
    }
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message
    );
echo json_encode($response);
?>