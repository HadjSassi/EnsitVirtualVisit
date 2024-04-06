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
// echo "Connected successfully, now we will show the users.<br><br>";

$sql = "select * from Affiches where existant = 1";

$result = $conn -> query($sql);

if ($result->num_rows > 0){
    while($row = $result -> fetch_assoc()){
        echo $row['idAffiche']." || ".
            $row['titre']." || ".
            $row['sujet']." || ".
            $row['description']." || ".
            $row['localisationAffiche']." || ".
            $row['image']." || ".
            $row['prix']." || ".
            $row['lien'].
            "<br>";
    }
} else {
    echo "0 results ";
}

$conn-> close();
?>
