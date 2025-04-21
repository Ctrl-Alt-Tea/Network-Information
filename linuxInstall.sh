#!/bin/bash

# This script installs the required packages for the project on a Linux system.
# It has been tested on Ubuntu or Debian-based systems.

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
    echo "Build successful!"
else
    echo "Build failed. Please check the output for errors."
    exit 1
fi



# Test Run the project
echo "Test Running the project..."
dotnet run