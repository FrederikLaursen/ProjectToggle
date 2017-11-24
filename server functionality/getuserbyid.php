<?php 
ini_set('display_errors', 'On');
error_reporting(E_ALL);

	$servername = "localhost";
	$username = "username";
	$password = "pw";
	$dbname = "dbname";

	$conn = new PDO("mysql:host=$servername;dbname=$dbname", $username, $password);
	
	$id = $_POST['id']; 									   // Get the id passed in URL
	$sql = "SELECT username FROM Users WHERE ID = '$id'";  // Setup the query
	$query = $conn->query($sql); 						   // Run query in db
	$result = $query->fetch(PDO::FETCH_ASSOC);  		   // Fetch the data
	echo $result['username']; 							   // Echo the username

$conn = null;
?>
