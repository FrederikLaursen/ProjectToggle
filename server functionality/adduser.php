<?php 
ini_set('display_errors', 'On');
error_reporting(E_ALL);

    try {
        $dbh = new PDO('mysql:host=localhost;dbname=DATABASENAME;charset=utf8mb4', 'root', 'DATABASEPASS');
    } catch(PDOException $e) {
        echo '<h1>An error has occurred.</h1><pre>', $e->getMessage() ,'</pre>';
    }

	$id = $_POST['id'];
	$name = $_POST['name'];
	$password = $_POST['password'];
	$email = $_POST['email'];
	
	$uniquechecker = $dbh->query("SELECT count(*) FROM `Users` WHERE username = '$name' OR email = '$email'")->fetchColumn();
	echo $uniquechecker;

	if ($uniquechecker == 0)
	{

		header('UserCreation: true'); 	
		$hashpw = password_hash($password, PASSWORD_DEFAULT);
		
		$sth = $dbh->prepare("INSERT INTO Users (id,username,password,email) VALUES (:id,:name,:password,:email)");
		
		$sth->bindParam(':id', $id);
		$sth->bindParam(':name', $name);
		$sth->bindParam(':password', $hashpw);
		$sth->bindParam(':email', $email);
		
		$sth->execute();
		$dbh = null;
	}
	else
	{
	header('UserCreation: false'); 	
	$dbh = null;	
	}
?>