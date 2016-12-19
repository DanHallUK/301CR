<?php


$db = "chessgam_leaderboard";//Database name
$dbu = "chessgam_dan";//Database username
$dbp = "q1w2e3r4";//Database password
$host = "localhost";//Host name

mysql_connect($host,$dbu,$dbp);
mysql_select_db($db);


     
     $jsonInput = file_get_contents('scores.json');//opens scores.json, and stores as string
     $json = json_decode($jsonInput , true);//converts the string into a associative array
     
     
     $counter = 0;//initialize counter variable
     foreach ($json['scoresArray'] as $value){ //loop to run through every element in the json file array
     	
     	 $names = $json['scoresArray'][$counter ]['name']; //assigns the variable name, based on the name set in json file
         $ratings= $json['scoresArray'][$counter ]['rating']; //assigns the variable rating, based on rating set in json file
         
         
         $sql = mysql_query("INSERT INTO `$db`.`scores` (`id`,`name`,`rating`) VALUES ('','$names','$ratings');"); //mysql query to insert name and rating into scores table on databse
         $counter = $counter + 1; //increment counter, so names and ratings assignment changes every itteration
     	
     }
     
     
     
     if($sql){
     
          //The query returned true
          echo 'Your score was saved. Congrats!';
          
     }else{
     
          //The query returned false
          echo 'There was a problem saving your score. Please try again later.';
          
}

//Close off the MySQL connection
mysql_close();
?>