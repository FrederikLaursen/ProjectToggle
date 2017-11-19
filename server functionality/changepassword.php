<?php
error_reporting(E_ALL);

	$servername = "localhost";
	$username = "root";
	$password = "unitydb123";
	$dbname = "Unityproject";

	$conn = new PDO("mysql:host=$servername;dbname=$dbname", $username, $password);
	
	// Call this script with:
	
	// User id.
	$id = $_POST['id']; 
	// Temporary password.
	$tempPw = $_POST['tempPass']; 
	// New password.
	$newPw = $_POST['newPass']; 
	

	// Verify that the temporary password is correct
	$sql = "SELECT password FROM Users WHERE ID = '$id'";  // Setup the query
	$query = $conn->query($sql); 						   // Run query in db
	$result = $query->fetch(PDO::FETCH_ASSOC);  		   // Fetch the data
	$storedPw = $result['password']; 					   // Get the password
	
	if(password_verify($tempPw,$storedPw))
	{
		$hashpass = password_hash($newPw, PASSWORD_DEFAULT);
		// Set the user password to the new password
		$sql = "UPDATE `Users` SET `password`='$hashpass' WHERE ID = '$id'";   // Setup the query
		$query = $conn->query($sql); 						   				   // Run query in db
		echo "Success";
	}
	else
		echo "Incorrect temporary password";
		

	$conn = null;
?>