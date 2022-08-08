using System;
namespace GymRecorderNETversion
{
    public class Exercise
    {
        private string exerciseName;
        private List<ExerciseInstant> iterations = new List<ExerciseInstant>();

        public Exercise(string exerciseName)
        {
            this.exerciseName = exerciseName;
        }

        private class ExerciseInstant
        {

        }
    }
}

