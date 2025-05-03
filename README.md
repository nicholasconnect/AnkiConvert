Description: Able to convert notes into CSV format readible for comma serperated multiple choice Anki.

1. Install Anki App
2. Install Anki Multiple Choice 10 solution add-on
3. Take note in format:
[Question]
- [Answer 1]
- [Answer 2]
- [Answer 3]
- [Answer 4] /

Note: Foreward slash denotes correct answers. 

Sample input:
Which of the following BEST describes the primary purpose of establishing rules of engagement when conducting a security assessment for a third-party vendor?

- Determining the financial costs of the security assessment.
    
- Setting the timeline for the next vendor agreement renewal.
    
- Listing the personnel who will be involved in the security assessment.
    
- Defining the boundaries and limitations during the assessment. /

Sample output: 
Question 1,"Which of the following BEST describes the primary purpose of establishing rules of engagement when conducting a security assessment for a third-party vendor?",2,"Determining the financial costs of the security assessment.","Setting the timeline for the next vendor agreement renewal.","Listing the personnel who will be involved in the security assessment.","Defining the boundaries and limitations during the assessment.",,,,,,,,,0 0 0 1,,

Note: Commas in questions and answers are deleted in the process. 

4. Place note in same in same file as executable with the name "input.txt".
5. Run executable file and verify the "output.csv" file is made.
6. Import output.csv to Anki, under note type be sure to use "multiple choice" add-on and choose seperator as "comma". 
