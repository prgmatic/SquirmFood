<?php
require('Connect.php');

$message = "";
$message .= "Connected to database...\r\n";

$validated = false;
$name = "";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key = StripInput($_POST["Key"]);
    
    $message .= "$key\r\n";
    
    $query = "SELECT Name FROM Players WHERE `Key`= '$key'";
    $queryResult = $conn->query($query);

    if($queryResult)
    {
        while($row = $queryResult->fetch_assoc())
        {
            $validated = true;
            $name = $row["Name"];
            $message .= "Key found...\r\n";
        }
    }
    else
    {
        $message .= "Key could not be found...\r\n";
    }
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
    "Message" => $message,
    "Validated" => $validated,
    "Name" => $name
);
    
echo json_encode($response);
?>