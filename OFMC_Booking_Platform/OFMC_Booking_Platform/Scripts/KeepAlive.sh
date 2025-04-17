#!/bin/bash
while true; do
	curl -s "https://ofmc.onrender.com" > /dev/null
	echo "Ping sent at $(date)"
	sleep 800
done