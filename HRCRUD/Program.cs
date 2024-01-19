using HRCRUD.BD;
using HRCRUD.Models;
using HRCRUD.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var context = new HRSystemContext();
            var authService = new AuthService(context);

            User user = null;
            do
            {
                Console.Write("Enter username: ");
                var username = Console.ReadLine();

                Console.Write("Enter password: ");
                var password = Console.ReadLine();

                user = authService.Login(username, password);
                if (user == null)
                {
                    Console.Clear();
                    Console.WriteLine("Wrong username or password. Please try again.");
                }
            } while (user == null);

            if (user.Role.RoleName == "Admin")
            {
                Console.Clear();
                Console.WriteLine("\nHello Admin!");
                ShowAdminActions(context);
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Welcome {user.Username}! Your role is: {user.Role.RoleName}.");
                ShowHRActions(context);
            }
        }

        // Logic for HR account
        static void ShowHRActions(HRSystemContext context)
        {
            while (true)
            {
                Console.WriteLine("\nPossible actions:");
                Console.WriteLine("HR Actions:");
                Console.WriteLine("1. View Current Vacancies");
                Console.WriteLine("2. View Candidates for a Vacancy");
                Console.WriteLine("3. Change Vacancy Status");
                Console.WriteLine("Type: 'Exit' for exit");

                Console.Write("\nEnter your choice: ");
                var choice = Console.ReadLine();

                if (choice.Equals("Exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ViewCurrentVacancies(context);
                        break;
                    case "2":
                        ViewCandidatesForVacancy(context);
                        break;
                    case "3":
                        ChangeVacancyStatus(context);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void ViewCurrentVacancies(HRSystemContext context)
        {
            var vacancies = context.Vacancies.ToList();
            Console.WriteLine("Current Vacancies:");
            foreach (var vacancy in vacancies)
            {
                Console.WriteLine($"Vacancy ID: {vacancy.VacancyId}, Title: {vacancy.Title}, Description: {vacancy.Description}, Status: {vacancy.Status}");
            }
        }

        static void ViewCandidatesForVacancy(HRSystemContext context)
        {
            Console.Write("Enter Vacancy ID: ");
            if (int.TryParse(Console.ReadLine(), out int vacancyId))
            {
                var applications = context.Applications
                                          .Include(a => a.Candidate)
                                          .Include(a => a.Vacancy)
                                          .Where(a => a.VacancyId == vacancyId)
                                          .ToList();

                Console.WriteLine($"Candidates for Vacancy ID {vacancyId}:");
                foreach (var application in applications)
                {
                    var candidate = application.Candidate;
                    Console.WriteLine($"Candidate ID: {candidate.CandidateId}, Name: {candidate.Name}, Age: {candidate.Age}, Skills: {candidate.Skills}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Vacancy ID.");
            }
        }


        static void ChangeVacancyStatus(HRSystemContext context)
        {
            Console.Write("Enter Vacancy ID: ");
            if (int.TryParse(Console.ReadLine(), out int vacancyId))
            {
                var vacancy = context.Vacancies.FirstOrDefault(v => v.VacancyId == vacancyId);

                if (vacancy != null)
                {
                    Console.WriteLine($"Current Status of Vacancy '{vacancy.Title}': {vacancy.Status}");
                    Console.WriteLine("Enter new status (e.g., Open, Closed): ");
                    if (Enum.TryParse(Console.ReadLine(), out VacancyStatus newStatus))
                    {
                        vacancy.Status = newStatus;
                        context.SaveChanges();
                        Console.WriteLine($"Vacancy '{vacancy.Title}' status updated to {newStatus}.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Status.");
                    }
                }
                else
                {
                    Console.WriteLine("Vacancy not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Vacancy ID.");
            }
        }










        // Logic for admin account
        static void ShowAdminActions(HRSystemContext context)
            {
                while (true)
                {
                    Console.WriteLine("\nPossible actions:");
                    Console.WriteLine("1. Add HR");
                    Console.WriteLine("2. Update HR");
                    Console.WriteLine("3. Delete HR");
                    Console.WriteLine("4. View HR List");
                    Console.WriteLine("Type: 'Exit' for exit");

                    Console.Write("\nEnter your choice: ");
                    var choice = Console.ReadLine();

                    if (choice.Equals("Exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Exiting...");
                        break;
                    }

                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            AddHR(context);
                            break;
                        case "2":
                            UpdateHR(context);
                            break;
                        case "3":
                            DeleteHR(context);
                            break;
                        case "4":
                            ViewHRList(context);
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
        }

        static void AddHR(HRSystemContext context)
        {
            Console.Write("Enter HR name: ");
            var hrName = Console.ReadLine();
            Console.Write("Enter Department name: ");
            var departmentName = Console.ReadLine();

            var department = context.Departments.FirstOrDefault(d => d.Name == departmentName);

            if (department != null)
            {
                var newHR = new HR { Name = hrName, DepartmentId = department.DepartmentId, Department = department };
                context.HRs.Add(newHR);
                context.SaveChanges();

                Console.WriteLine($"HR '{hrName}' in department '{departmentName}' has been added.");
            }
            else
            {
                Console.WriteLine($"Department '{departmentName}' not found.");
            }
        }

        static void UpdateHR(HRSystemContext context)
        {
            Console.Write("Enter HR name to update: ");
            var hrName = Console.ReadLine();
            Console.Write("Enter new HR name: ");
            var newHrName = Console.ReadLine();
            Console.Write("Enter new Department name: ");
            var newDepartmentName = Console.ReadLine();

            var hrToUpdate = context.HRs.Include(h => h.Department).FirstOrDefault(h => h.Name == hrName);
            var newDepartment = context.Departments.FirstOrDefault(d => d.Name == newDepartmentName);

            if (hrToUpdate != null && newDepartment != null)
            {
                hrToUpdate.Name = newHrName;
                hrToUpdate.DepartmentId = newDepartment.DepartmentId;
                hrToUpdate.Department = newDepartment;

                context.SaveChanges();
                Console.WriteLine($"HR '{hrName}' has been updated to '{newHrName}' in department '{newDepartmentName}'.");
            }
            else
            {
                Console.WriteLine("HR or Department not found.");
            }
        }

        static void DeleteHR(HRSystemContext context)
        {
            Console.Write("Enter HR name to delete: ");
            var hrName = Console.ReadLine();

            var hrToDelete = context.HRs.FirstOrDefault(h => h.Name == hrName);

            if (hrToDelete != null)
            {
                context.HRs.Remove(hrToDelete);
                context.SaveChanges();
                Console.WriteLine($"HR '{hrName}' has been deleted.");
            }
            else
            {
                Console.WriteLine("HR not found.");
            }
        }

        static void ViewHRList(HRSystemContext context)
        {
            var hrList = context.HRs.Include(h => h.Department).ToList();

            Console.WriteLine("HR List:");
            foreach (var hr in hrList)
            {
                Console.WriteLine($"HR Name: {hr.Name}, Department: {hr.Department.Name}");
            }
        }














































































        // Template for init DataBase. Pls do not call this func
        public static void InitDataBaseForTZ()
        {
            using (var context = new HRSystemContext())
            {
                var departments = context.Departments.ToList();

                context.Departments.AddRange(departments);

                context.SaveChanges();

                var hrList = context.HRs.ToList();

                context.HRs.AddRange(hrList);

                context.SaveChanges();

                AddVacanciesAndCandidates(context);

                var adminRole = new Role { RoleName = "Admin" };
                var hrRole = new Role { RoleName = "HR" };

                context.Roles.AddRange(new[] { adminRole, hrRole });

                AddUsers(context, adminRole, hrRole);

                context.SaveChanges();
            }
        }

        private static void AddUsers(HRSystemContext context, Role adminRole, Role hrRole)
        {
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = "admin123",
                Role = adminRole
            };

            var hrUser = new User
            {
                Username = "hr",
                PasswordHash = "hr123",
                Role = hrRole
            };

            context.Users.AddRange(new[] { adminUser, hrUser });
        }

        private static void AddVacanciesAndCandidates(HRSystemContext context)
        {
            var vacancy1 = new Vacancy
            {
                Title = "Developer",
                Description = "Create Soft",
                Department = context.Departments.First(),
                DepartmentId = context.Departments.First().DepartmentId,
                HR = context.HRs.First(),
                HRId = context.HRs.First().HRId
            };
            var vacancy2 = new Vacancy
            {
                Title = "Designer",
                Description = "Create Design",
                Department = context.Departments.First(),
                DepartmentId = context.Departments.First().DepartmentId,
                HR = context.HRs.Skip(1).First(),
                HRId = context.HRs.Skip(1).First().HRId
            };

            context.Vacancies.AddRange(new[] { vacancy1, vacancy2 });
            context.SaveChanges();

            var candidates = ParseFromHHRu();
            foreach (var candidate in candidates)
            {
                context.Candidates.Add(candidate);

                if (candidate.Name == "Svetlana")
                {
                    candidate.Applications.Add(new Application { Vacancy = vacancy1, VacancyId = vacancy1.VacancyId, Candidate = candidate, CandidateId = candidate.CandidateId });
                }
                else
                {
                    candidate.Applications.Add(new Application { Vacancy = vacancy2, VacancyId = vacancy2.VacancyId, Candidate = candidate, CandidateId = candidate.CandidateId });
                    candidate.TestTasks.Add(new TestTask
                    {
                        Description = "Тестовое задание",
                        Complexity = TaskComplexity.Medium,
                        Candidate = candidate
                    });
                }
            }

            context.SaveChanges();
        }


        private static List<Department> CreateDepartments()
        {
            return new List<Department>
            {
                new Department { Name = "IT", Description = "Information Technology" },
                new Department { Name = "HR", Description = "Human Resources" },
                new Department { Name = "Finance", Description = "Financial Department" },
                new Department { Name = "Marketing", Description = "Marketing and Sales" },
                new Department { Name = "Operations", Description = "Operations Management" },
                new Department { Name = "Legal", Description = "Legal Affairs" },
                new Department { Name = "Research", Description = "Research and Development" },
                new Department { Name = "Customer Service", Description = "Customer Support and Services" },
                new Department { Name = "Executive", Description = "Executive Management" },
                new Department { Name = "Production", Description = "Production and Manufacturing" }
            }; 
        }

        private static List<Candidate> ParseFromHHRu()
        {
            return new List<Candidate>
            {
                new Candidate { Name = "Ivan", Age = 30, Skills = "C#, .NET" },
                new Candidate { Name = "Maria", Age = 28, Skills = "JavaScript, React" },
                new Candidate { Name = "Alex", Age = 35, Skills = "Java, Spring" },
                new Candidate { Name = "Svetlana", Age = 32, Skills = "Python, Django" },
                new Candidate { Name = "Dmitry", Age = 29, Skills = "C++, Qt" },
                new Candidate { Name = "Helena", Age = 27, Skills = "UI/UX Design, Adobe Photoshop" }
            };
        }
    }
}
