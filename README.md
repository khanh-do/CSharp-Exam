# CSharp-Exam
This web application allows registered users to schedule a new activity or join/leave a previously scheduled activity. The dashboard displays scheduled activities with the name of the activity, the date and time, the duration, the first name of the activity coordinator (creator of the activity), the total number of participants, and the available options for the user. The available options depend on the user. If the user had created the activity, they will have the option to delete the activity from the list. If the user is not participant of an activity, they will have the option of joining it. If the user has joined the activity, they will have the option of leaving it. Clicking on the name of the activity will display the details of the activity including the name of the event coordinator, the description of the activity and a list of the current participants.

Validations:
Login/Reg: This application implements a full range of login and registration validations. This includes the following: All fields are required. The user first name and last name must be at least 2 characters in length. The user email must be unique and be in a valid format. The password must contain at least one upper case english letter, at least one lower case english letter, at least one digit, and at least one special character and be a minimum of 8 characters in length.
Activity: Users are not allowed to create activities that occurred in the past. In addition, the dashboard does not display activities that were saved but have already passed. All fields are required. The title and the description of the activity must be at least 2 characters in length. 

Technologies:
C#, ASP.NET Core MVC framework, Entity Framework, LINQ, MySQL, Windows Session, nginx, AWS EC2

URL:
http://3.17.59.62/
