<?php 

$servername = "localhost";
$username = "root";
$password = "Magali_1984";
$dbname = "unitytuto";

//create connection 
$conn = new mysqli($servername,$username,$password, $dbname);

//check connection
if($conn -> connect_error){
    die("Connection Failed: ".$conn -> connect_error);
}
echo "Connected successfully, now we will show the users.<br><br>";

$sql = "select username, level from users";

$result = $conn -> query($sql);

if ($result->num_rows > 0){
    while($row = $result -> fetch_assoc()){
        echo "username: ".$row["username"]." - level: ".$row["level"]."<br>";
    }
} else {
    echo "0 results ";
}

$conn-> close();
?>
