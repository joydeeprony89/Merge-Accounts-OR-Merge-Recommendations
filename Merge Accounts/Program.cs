using System;
using System.Collections.Generic;
using System.Linq;

namespace Merge_Accounts
{
    class Program
    {
        static void Main(string[] args)
        {
            var accounts = new List<IList<string>>();
            accounts.Add(new List<string> { "John", "johnsmith@mail.com", "john_newyork@mail.com" });
            accounts.Add(new List<string> { "John", "johnsmith@mail.com", "john00@mail.com" });
            accounts.Add(new List<string> { "Mary", "mary@mail.com" });
            accounts.Add(new List<string> { "John", "johnnybravo@mail.com" });
            Program p = new Program();
            var result = p.AccountsMerge(accounts);
            foreach (var res in result)
                Console.WriteLine(string.Join(",", res));
        }

        readonly Dictionary<int, HashSet<string>> accountToMails = new Dictionary<int, HashSet<string>>();
        readonly Dictionary<string, HashSet<int>> emailsToAccount = new Dictionary<string, HashSet<int>>();

        // runtime complexity= 
        IList<IList<string>> AccountsMerge(IList<IList<string>> accounts)
        {
            IList<IList<string>> result = new List<IList<string>>();
            if (accounts == null || accounts.Count == 0) return result;
            CreateGraph(accounts);
            HashSet<int> visited = new HashSet<int>();
            for (int i = 0; i < accounts.Count; i++)
            {
                if (visited.Contains(i)) continue;

                HashSet<string> singleAccountEmails = new HashSet<string>();
                DFS(i, visited, singleAccountEmails);
                string name = accounts[i][0];
                var emails = singleAccountEmails.ToList();
                emails.Sort(StringComparer.Ordinal);
                emails.Insert(0, name);
                result.Add(emails);
            }

            return result;
        }

        void CreateGraph(IList<IList<string>> accounts)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accountToMails.ContainsKey(i))
                    continue;

                accountToMails.Add(i, new HashSet<string>());
                for (int j = 1; j < accounts[i].Count; j++)
                {
                    string mail = accounts[i][j];
                    accountToMails[i].Add(mail);
                    if (!emailsToAccount.ContainsKey(mail))
                        emailsToAccount.Add(mail, new HashSet<int>());
                    emailsToAccount[mail].Add(i);
                }
            }
        }

        void DFS(int account, HashSet<int> visited, HashSet<string> singleAccountEmails)
        {
            if (visited.Contains(account)) return;
            visited.Add(account);
            var emails = accountToMails[account];
            foreach (string mail in emails)
            {
                singleAccountEmails.Add(mail);
                var accounts = emailsToAccount[mail];
                foreach(int acc in accounts)
                {
                    if (visited.Contains(acc)) continue;
                    DFS(acc, visited, singleAccountEmails);
                }
            }
        }
    }
}
