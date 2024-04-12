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

// Check if the 'url' parameter is set
if (isset($_GET['url'])) {
    // Get the URL parameter value
    $url = $_GET['url'];

    // Prepare a SQL statement to select avatar details based on the URL
    $sql = "SELECT * FROM Avatar WHERE url = '$url'";

    // Execute the SQL statement
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        // Output data of each row
        while ($row = $result->fetch_assoc()) {
            echo  $row['avatarName'] . " || " .
                  $row['description'] . " || " .
                  $row['jokes'] . " || " .
                  $row['existant'] . " || " .
                  $row['sexe'] . " || " .
                 $row['mail'] . "<br>";
        }
    } else {
        echo "No results found for the provided URL.";
    }
} else {
    // Prepare a SQL statement to select avatar details based on the URL
        $sql = "SELECT * FROM Avatar WHERE mail = '' ORDER BY RAND()";

                // Execute the SQL statement
                $result = $conn->query($sql);

                if ($result->num_rows > 0) {
                    // Output data of each row
                    while ($row = $result->fetch_assoc()) {
                        echo  $row['url'] . " || " .
                              $row['avatarName'] . " || " .
                              $row['description'] . " || " .
                              $row['jokes'] . " || " .
                              $row['existant'] . " || " .
                              $row['sexe'] . " || " .
                             $row['mail'] . "<br>";
                    }
                }

        echo "-*-*-";
        $sql = "SELECT * FROM Avatar WHERE mail != '' ORDER BY RAND()";

                // Execute the SQL statement
                $result = $conn->query($sql);

                if ($result->num_rows > 0) {
                    // Output data of each row
                    while ($row = $result->fetch_assoc()) {
                        echo  $row['url'] . " || " .
                              $row['avatarName'] . " || " .
                              $row['description'] . " || " .
                              $row['jokes'] . " || " .
                              $row['existant'] . " || " .
                              $row['sexe'] . " || " .
                             $row['mail'] . "<br>";
                    }
                }
}

// Close connection
$conn->close();

?>
