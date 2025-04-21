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
    echo "Build successful!"
else
    echo "Build failed. Please check the output for errors."
    exit 1
fi

# Add alias to .bashrc
echo "alias network='./home/$USER/Network-Information/bin/Debug/net8.0/Network # Your path to Network-Information'" >> ~/.bashrc

# Test Run the project (For testing purposes)
#echo "Test Running the project..."
#dotnet run