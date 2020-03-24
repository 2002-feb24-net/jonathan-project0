# Project 0 - Jonathan Bui

## Dependencies/Environment
- Visual Studio 2019
- SQL Server Management Studio
- .NET Core

## Environment Setup
- Follow the instructions in WheyMeny/scaffoldbashcommand.txt

## Directory Structure
- Validation Testing
- WheyMen 
- WheyMenDAL.Library
  - AOD_Interfaces
  - DAL
  - Model
    - AOD_Models    
- WheyMenIOValidation.Library

#### Validation Testing
Contains Xunit batch test files. 
#### WheyMen 
Contains IO logic for console app
#### WheymenDAL.Library 
Contains all Data access related implementations
##### DAL
Contains DAL impleemntations, handles direct retrieval,insertions to database
##### Model
Contains scaffold generated representations of database tables

####AOD_* 
Deprecated files implementing the project using pure AOD.NET objects. 

