#!/bin/bash

# This script installs the required packages for the project on a Linux system.
# It has been tested on Ubuntu or Debian-based systems.

cd

# Update the package list
sudo apt update -y

# Install required packages
sudo apt install dotnet8 git -y

# Install additional dependencies
git clone https://github.com/Ctrl-Alt-Tea/Network-Information.git



# Build the project
cd Network-Information
dotnet build

# Check if the build was successful
if [ $? -eq 0 ]; then
    echo " "
    echo "Build successful!"
    echo " "
else
    echo " "
    echo "Build failed. Please check the output for errors."
    echo " "
    exit 1
fi

# Add alias to .bashrc
echo "alias network='./Network-Information/bin/Debug/net8.0/Network'" >> ~/.bashrc
echo "Updated .bashrc restart terminal ..."
echo "After restarting the terminal, you can run the project using the command 'network'"
echo " "
# Test Run the project (For testing purposes)
#echo "Test Running the project..."
#dotnet run