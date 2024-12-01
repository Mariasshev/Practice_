using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ConsoleApp11
{
    public class Quiz
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public string Login { get; set; }

        //+
        public bool CheckLogin(string login, string passwd)
        {
            string filePath = "logins.txt";
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        if(parts[0] == login && parts[1] == passwd)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //+
        public void Registration()
        {
            Console.Write("Введите логин: ");
            string userLogin = Console.ReadLine();
            Login = userLogin;
            Console.Write("Введите пароль: ");
            string userPasswd = Console.ReadLine();

            string userBirth;
            while (true)
            {
                Console.Write("Введите дату рождения (ДД.ММ.ГГГГ): ");
                userBirth = Console.ReadLine();

                if (DateTime.TryParseExact(userBirth, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    break; 
                }
                else
                {
                    Console.WriteLine("Некорректный формат даты. Пожалуйста, введите дату в формате ДД.ММ.ГГГГ.");
                }
            }


            string filePath = "logins.txt";
            string line = $"{userLogin},{userPasswd},{userBirth}";

            File.AppendAllText(filePath, line + Environment.NewLine);

            if (CheckLogin(userLogin, userPasswd))
            {
                Console.WriteLine("Учетная запись уже существует! Пожалуйста, войдите");
            }
            else
            {
                ShowMenu();
            }

        }

        //+
        public void LoadQuestionsFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 3)
                    {
                        Questions.Add(new Question
                        {
                            Text = parts[0].Trim(),
                            Options = parts[1].Split(',').Select(option => option.Trim()).ToList(), 
                            CorrectAnswer = parts[2].Trim() 
                        });
                    }
                }
            }
        }

        //+
        public void SaveResult(string userName, string quizName, int score, int totalQuestions)
        {
            string resultsFilePath = "quiz_results.txt";
            string resultLine = $"{userName} | {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {quizName} | {score}/{totalQuestions},";

            File.AppendAllText(resultsFilePath, resultLine + Environment.NewLine);
        }

        //+
        public void ViewPreviousResults()
        {
            string resultsFilePath = "quiz_results.txt";

            if (!File.Exists(resultsFilePath))
            {
                Console.WriteLine("Результаты отсутствуют.");
                return;
            }

            Console.WriteLine("\nПредыдущие результаты:");
            
            var lines = File.ReadAllLines(resultsFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                
                if (parts[0].Contains(Login))
                {
                    Console.WriteLine(parts[0]);
                }
            }
        }

        //+
        public void ShowTop20()
        {
            string resultsFilePath = "quiz_results.txt";

            if (!File.Exists(resultsFilePath))
            {
                Console.WriteLine("Результаты отсутствуют.");
                return;
            }

            Console.WriteLine("\nТОП-20 прохождений:");

            
        }

        //+
        public void HistoryQuiz()
        {
            Console.WriteLine("Добро пожаловать на викторину по истории!");
            Console.WriteLine("Всего 20 вопросов. Начинаем!");

            string filePath = "history.txt";
            LoadQuestionsFromFile(filePath);

            if(Questions.Count == 0)
            {
                Console.WriteLine("Вопросы отсутствуют!");
                return;
            }

            int userScore = 0;

            for (int i = 0; i < Questions.Count; i++) { 
                var question = Questions[i];
                Console.WriteLine($"Вопрос {i+1}: {question.Text} ");
                for (int j = 0; j < question.Options.Count; j++)
                {
                    Console.WriteLine($"{j + 1}) {question.Options[j]}");
                }
                Console.Write("Напишите ответ:");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int userAnswer) && userAnswer >= 1 && userAnswer <= question.Options.Count)
                {
                    if (question.Options[userAnswer - 1] == question.CorrectAnswer)
                    {
                        Console.WriteLine("Правильно!");
                        userScore++;
                    }
                    else
                    {
                        Console.WriteLine($"Неправильно! Ваш ответ: {question.Options[userAnswer - 1]}, правильный ответ: {question.CorrectAnswer}");
                    }

                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Ответ не засчитан.");
                }
                Thread.Sleep(3000);
                Console.Clear();
            }

            Console.WriteLine($"\nВикторина завершена! Ваш результат: {userScore} из {Questions.Count}.");
            SaveResult(Login, "История", userScore, Questions.Count);
            Thread.Sleep(3000);
            Console.Clear();
            ShowMenu();

        }

        //+
        public void GeographyQuiz()
        {
            Console.WriteLine("Добро пожаловать на викторину по географии!");
            Console.WriteLine("Всего 20 вопросов. Начинаем!");

            string filePath = "geography.txt";
            LoadQuestionsFromFile(filePath);

            if (Questions.Count == 0)
            {
                Console.WriteLine("Вопросы отсутствуют!");
                return;
            }

            int userScore = 0;

            for (int i = 0; i < Questions.Count; i++)
            {
                var question = Questions[i];
                Console.WriteLine($"Вопрос {i + 1}: {question.Text} ");
                for (int j = 0; j < question.Options.Count; j++)
                {
                    Console.WriteLine($"{j + 1}) {question.Options[j]}");
                }
                Console.Write("Напишите ответ:");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int userAnswer) && userAnswer >= 1 && userAnswer <= question.Options.Count)
                {
                    if (question.Options[userAnswer - 1] == question.CorrectAnswer)
                    {
                        Console.WriteLine("Правильно!");
                        userScore++;
                    }
                    else
                    {
                        Console.WriteLine($"Неправильно! Ваш ответ: {question.Options[userAnswer - 1]}, правильный ответ: {question.CorrectAnswer}");
                    }

                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Ответ не засчитан.");
                }
                Thread.Sleep(3000);
                Console.Clear();
            }

            Console.WriteLine($"\nВикторина завершена! Ваш результат: {userScore} из {Questions.Count}.");
            SaveResult(Login, "География", userScore, Questions.Count);
            Thread.Sleep(3000);
            Console.Clear();
            ShowMenu();

        }

        //+
        public void BiologyQuiz()
        {
            Console.WriteLine("Добро пожаловать на викторину по биологии!");
            Console.WriteLine("Всего 20 вопросов. Начинаем!");

            string filePath = "biology.txt";
            LoadQuestionsFromFile(filePath);

            if (Questions.Count == 0)
            {
                Console.WriteLine("Вопросы отсутствуют!");
                return;
            }

            int userScore = 0;

            for (int i = 0; i < Questions.Count; i++)
            {
                var question = Questions[i];
                Console.WriteLine($"Вопрос {i + 1}: {question.Text} ");
                for (int j = 0; j < question.Options.Count; j++)
                {
                    Console.WriteLine($"{j + 1}) {question.Options[j]}");
                }


                Console.WriteLine("Выберите правильные ответы (через запятую, например: 1, 3, 4):");
                string userInput = Console.ReadLine();

                var userAnswers = userInput.Split(',').Select(a => a.Trim()).ToList();
                bool isCorrect = true;

                foreach (var userAnswer in userAnswers)
                {
                    if (int.TryParse(userAnswer, out int index) && index >= 1 && index <= question.Options.Count)
                    {
                        if (!question.CorrectAnswer.Split(',').Any(correctAnswer => correctAnswer.Trim() == question.Options[index - 1]))
                        {
                            isCorrect = false;
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ввод. Ответ не засчитан.");
                        isCorrect = false;
                        break;
                    }
                }

                if (isCorrect)
                {
                    Console.WriteLine("Правильно!");
                    userScore++;
                }
                else
                {
                    Console.WriteLine($"Неправильно! Правильный ответ: {string.Join(", ", question.CorrectAnswer.Split(','))}");
                }
                Thread.Sleep(3000);
                Console.Clear();
            }

            Console.WriteLine($"\nВикторина завершена! Ваш результат: {userScore} из {Questions.Count}.");
            SaveResult(Login, "Биология", userScore, Questions.Count);
            Thread.Sleep(3000);
            Console.Clear();
            ShowMenu();
        }

        //+
        public void MixQuiz()
        {
            Console.WriteLine("Добро пожаловать на смешанную викторину!");
            Console.WriteLine("Всего 15 вопросов. Начинаем!");

            string[] files = { "history.txt", "geography.txt", "biology.txt" };
            List<Question> allQuestions = new List<Question>();

            foreach (var file in files)
            {
                LoadQuestionsFromFile(file);
            }

            Random random = new Random();
            allQuestions = allQuestions.OrderBy(x => random.Next()).ToList();

            allQuestions = allQuestions.Take(15).ToList();

            if (allQuestions.Count < 15)
            {
                Console.WriteLine("Недостаточно вопросов для викторины. Пожалуйста, добавьте больше вопросов в файлы.");
                return;
            }

            int userScore = 0;

            for (int i = 0; i < allQuestions.Count; i++)
            {
                var question = allQuestions[i];
                Console.WriteLine($"Вопрос {i + 1}: {question.Text} ");
                for (int j = 0; j < question.Options.Count; j++)
                {
                    Console.WriteLine($"{j + 1}) {question.Options[j]}");
                }
                Console.Write("Напишите ответ: ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int userAnswer) && userAnswer >= 1 && userAnswer <= question.Options.Count)
                {
                    if (question.Options[userAnswer - 1] == question.CorrectAnswer)
                    {
                        Console.WriteLine("Правильно!");
                        userScore++;
                    }
                    else
                    {
                        Console.WriteLine($"Неправильно! Ваш ответ: {question.Options[userAnswer - 1]}, правильный ответ: {question.CorrectAnswer}");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Ответ не засчитан.");
                }
                Thread.Sleep(3000);
                Console.Clear();
            }

            Console.WriteLine($"\nВикторина завершена! Ваш результат: {userScore} из {allQuestions.Count}.");
            SaveResult(Login, "Смешанная викторина", userScore, allQuestions.Count);
            Thread.Sleep(3000);
            Console.Clear();
            ShowMenu();
        }

        //+
        public void StartQuiz()
        {
            Console.WriteLine("Выберите категорию: \n[1] - История \n[2] - География\n[3] - Биология\n[4] - Смешанная");
            int userChoice = int.Parse(Console.ReadLine());
            switch (userChoice) {
                case 1:
                    HistoryQuiz();
                    break;
                case 2:
                    GeographyQuiz();
                    break;
                case 3:
                    BiologyQuiz();
                    break;
                case 4:
                    MixQuiz();
                    break;
                default:
                    Console.WriteLine("Неккоректный вариант. Выберите другую категорию");
                    StartQuiz();
                    break;
            }

        }

        //+
        public void ChangeData()
        {
            Console.WriteLine("Что вы хотите изменить?\n[1] - логин\n[2] - пароль\n[3] - дату рождения");
            int answ = int.Parse(Console.ReadLine());
            string filePath = "logins.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл с логинами не найден.");
                return;
            }

            var lines = File.ReadAllLines(filePath).ToList();
            bool isChanged = false;

            switch (answ)
            {
                case 1:
                    Console.WriteLine("Введите старый логин: ");
                    string oldLogin = Console.ReadLine();

                    for (int i = 0; i < lines.Count; i++)
                    {
                        var parts = lines[i].Split(',');
                        if (parts.Length == 2 && parts[0] == oldLogin)
                        {
                            Console.WriteLine("Введите новый логин: ");
                            string newLogin = Console.ReadLine();
                            parts[0] = newLogin;
                            lines[i] = string.Join(",", parts);
                            isChanged = true;
                            Console.WriteLine("Логин успешно изменен.");
                            break;
                        }
                    }
                    break;
                case 2:
                    Console.WriteLine("Введите логин для изменения пароля: ");
                    string loginForPassword = Console.ReadLine();

                    for (int i = 0; i < lines.Count; i++)
                    {
                        var parts = lines[i].Split(',');
                        if (parts.Length == 2 && parts[0] == loginForPassword)
                        {
                            Console.WriteLine("Введите новый пароль: ");
                            string newPassword = Console.ReadLine();
                            parts[1] = newPassword;
                            lines[i] = string.Join(",", parts);
                            isChanged = true;
                            Console.WriteLine("Пароль успешно изменен.");
                            break;
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine("Введите логин для изменения даты рождения: ");
                    string loginForDateOfBirth = Console.ReadLine();

                    for (int i = 0; i < lines.Count; i++)
                    {
                        var parts = lines[i].Split(',');
                        if (parts.Length == 3 && parts[0] == loginForDateOfBirth)
                        {
                            Console.WriteLine("Введите новую дату рождения (формат: DD/MM/YYYY): ");
                            string newDateOfBirth = Console.ReadLine();
                            parts[2] = newDateOfBirth;
                            lines[i] = string.Join(",", parts);
                            isChanged = true;
                            Console.WriteLine("Дата рождения успешно изменена.");
                            break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Неверный выбор, повторите попытку");
                    ChangeData();
                    return;
            }

            if (isChanged)
            {
                File.WriteAllLines(filePath, lines);
            }
        }

        public void ShowMenu()
        {
            Console.WriteLine("Меню");
            Console.WriteLine("1 - Начать викторину");
            Console.WriteLine("2 - Мои предыдущие результаты");
            Console.WriteLine("3 - Показать ТОП-20");
            Console.WriteLine("4 - Настройки ");
            Console.WriteLine("5 - Выйти из программы ");

            Console.Write("Введите вариант: ");
            int userChoice = int.Parse(Console.ReadLine());

            switch (userChoice) {
                case 1:
                    StartQuiz();
                    break;

                case 2:
                    ViewPreviousResults();
                    break;

                case 3:
                    ShowTop20();
                    break;

                case 4:
                    ChangeData();
                    break;

                case 5:
                    Console.Clear();
                    Thread.Sleep(2000);
                    Console.WriteLine("Выход из программы....");
                    break;

                default:
                    Console.WriteLine("Неверный вариант");
                    ShowMenu();
                    break;

            }
        }


        public void CheckAuth()
        {
            Console.WriteLine("Вы зарегестрированы?\n [0] - да [1] - нет");
            int userCheckAuth = int.Parse(Console.ReadLine());
            Console.Clear();

            switch (userCheckAuth) {
                case 0:
                    Console.Write("Введите ваш логин : ");
                    string userLogin = Console.ReadLine();
                    Console.Write("Введите пароль: ");
                    string userPasswd = Console.ReadLine();
                    if(CheckLogin(userLogin, userPasswd))
                    {
                        Login = userLogin;
                        ShowMenu();
                    }
                    else
                    {
                        Console.WriteLine("Ваша учетная запись не найдена, пожалуйста, зарегистрируйтесь!");
                        Registration();
                    }
                    break;
                case 1:
                    Registration();
                    break;
                default:
                    Console.WriteLine("Неккоректные данные!");
                    CheckAuth();
                    break;
            }
            
        }


    }

    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }

        public bool CheckAnswer(string userAnswer)
        {
            return userAnswer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        }}

    class Program
    {
        static void Main(string[] args)
        {
            Quiz quiz = new Quiz();

            Console.WriteLine("Добро пожаловать в викторину!");
            quiz.CheckAuth();

        }
    }
}
