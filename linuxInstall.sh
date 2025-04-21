#!/bin/bash

# Project: Network Information
# Author: Ctrl-Alt-Tea
# Date: 21/04/2025
# Description: This script installs the required packages for the project on a Linux system.
# It installs .NET 8 and Git, clones the project repository, builds the project, and adds an alias to the .bashrc file for easy access.

# This install script has been tested on Ubuntu or Debian-based systems.

cd # Ensures we are in the home directory
sudo apt update -y # Update the package list
# Install required packages
sudo apt install dotnet8 git -
# Install project dependencies
git clone https://github.com/Ctrl-Alt-Tea/Network-Information.git



# Build the project
cd Network-Information
dotnet build

# Test Run the project (For testing purposes)
####echo "Test Running the project..."
####dotnet run

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
echo "Thank you for using Network Information!"
echo " "