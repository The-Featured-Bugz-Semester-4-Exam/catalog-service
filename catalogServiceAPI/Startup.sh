#/usr/bin/bash
export server="localhost"
export port="27017"
export connectionString="mongodb://localhost:27017/"
export database="ItemsDB"
export collection="Items"
export connAuk="auction-service:5158"
echo $database $connectionString
dotnet run server="$server" port="$port"

#/  ". ./startup.sh"