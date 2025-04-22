#!/bin/bash

# Project: Network Information
# Author: Ctrl-Alt-Tea
# Date: 21/04/2025
# Description: This script installs the required packages for the project on a Linux system.
# It installs .NET 8 and Git, clones the project repository, builds the project, and adds an alias to the .bashrc file for easy access.

# This install script has been tested on Ubuntu or Debian-based systems.

# Define color codes
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

cd # Ensures we are in the home directory
echo -e "${CYAN}Updating package list...${NC}"
sudo apt update -y # Update the package list

# Install required packages
echo -e "${CYAN}Installing required packages (dotnet8 and git)...${NC}"
sudo apt install dotnet8 git -y

# Clone the project repository
echo -e "${CYAN}Cloning the project repository...${NC}"
git clone https://github.com/Ctrl-Alt-Tea/Network-Information.git

# Build the project
echo -e "${CYAN}Building the project...${NC}"
cd Network-Information
dotnet build

# Check if the build was successful
if [ $? -eq 0 ]; then
    echo -e "${GREEN}Build successful!${NC}"
else
    echo -e "${RED}Build failed. Please check the output for errors.${NC}"
    exit 1
fi

# Add alias to .bashrc
echo -e "${CYAN}Adding alias to .bashrc...${NC}"
echo "alias network='./Network-Information/bin/Debug/net8.0/Network'" >> ~/.bashrc
echo -e "${YELLOW}Updated .bashrc. Restart your terminal to apply changes.${NC}"
echo -e "${GREEN}After restarting the terminal, you can run the project using the command 'network'.${NC}"
echo -e "${CYAN}Thank you for using Network Information!${NC}"