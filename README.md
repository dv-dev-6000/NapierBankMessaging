# Napier Bank Messaging
<div align="justify"> A WPF application developed as part of a university coursework assignment. The software is a message filtering application which is designed to receive, process and filter messages in multiple formats, the app detects the type of message received before scanning for hashtags, abbreviations, mentions and URLs. Hashtags and mentions are logged, URLs are blocked and abbreviations are converted to their full text format, messages are then serialised and saved. the application supports bulk processing of messages as well as allowing users to review data such as blocked URL lists and top mentions/hashtags. 
<br><br>
This project was undertaken as part of a Software Engineering module at Edinburgh Napier University. The project involved working through the whole software development lifecycle involving elements of project management, planning, design, development, testing and review. Test strategies for unit and integration testing were implemented and planning consisted of UML diagrams such as use case, activity and class diagrams. 

## Software Features

* detect incoming message types
* identify and log mentions & hashtags
* identify and expand abbreviations
* identify and block URLs
* display data to user
* serialise and save messages as JSON format
* process single message
* process bulk messages
* manual message input
* user friendly UI
