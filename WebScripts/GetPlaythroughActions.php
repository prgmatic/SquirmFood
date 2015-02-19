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
        $query = "SELECT Actions FROM Playthroughs WHERE id = $id";
        $queryResult = $conn->query($query);

        if($queryResult)
        {
            $message .= "Actions found...\r\n";
            while($row = $queryResult->fetch_assoc())
            {
                $actions = $row["Actions"];
            }
        }
        else
        {
            $message .= "Could not find playthrough.\r\n";
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