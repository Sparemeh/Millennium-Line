using System;
public class Quest
{
    public String QuestTitle;
    public String Description;
    public bool isCompleted;

    public void quest(string title, String description) {
        this.QuestTitle = title;
        this.Description = description;
        this.isCompleted = false;
    }

    public void completeTask() {
        this.isCompleted = true;
    }


}

