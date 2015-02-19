<?php
require('Connect.php');

$actions = "";
$message = "";
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $id                  = StripInput($_POST["PlaythroughID"]);
    $accessPassword      = StripInput($_POST["Password"]);
    
    if($accessPassword == $expectedAccessPassword)
    {
        $query = "DELETE FROM Playthroughs WHERE id = $id";
        $queryResult = $conn->query($query);

        if($queryResult)
        {
            $message .= "Playthrough Deleted...\r\n";
        }
        else
        {
            $message .= "Deletion failed...\r\n";
        }
    }
    else
        $message .= "Password Invalid";
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'Actions' => $actions
    );
echo json_encode($response);
?>