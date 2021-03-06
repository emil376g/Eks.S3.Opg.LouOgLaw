﻿using LouvOgRathApp.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LouvOgRathApp.ServerSide.DataAccess
{
    public class CasesRepository : RepositoryBase
    {
        #region contructer
        public CasesRepository(string nameOfConfigFileConnectionString) : base(nameOfConfigFileConnectionString)
        {
            
        }
        #endregion


        #region methods
        /// <summary>
        /// gets all cases from the database
        /// </summary>
        /// <returns></returns>
        public List<Case> GetAllCase()
        {
            List<Case> cases = new List<Case>();
            DataSet ds = Executor.Execute("SELECT * FROM GetCaseNameAndCaseKind");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Person clientPerson = new Person(dr.Field<string>("ClientName"), RoleKind.Client, dr.Field<int>("ClientId"));
                Person lawyerPerson = new Person(dr.Field<string>("LawyerName"), RoleKind.Lawyer, dr.Field<int>("LawyerId"));
                Person secretaryPerson = new Person(dr.Field<string>("SecretaryName"), RoleKind.Secretary, dr.Field<int>("SecretaryId"));
                Case tempCase = new Case(dr.Field<string>("CaseName"), (CaseKind)Enum.Parse(typeof(CaseKind), dr.Field<string>("CaseKind")), secretaryPerson, lawyerPerson, clientPerson, dr.Field<int>("CaseId"));
                cases.Add(tempCase);
            }
            return cases;
        }
        /// <summary>
        /// creating a new case in the database
        /// </summary>
        /// <param name="case"></param>
        public void AddCase(Case @case)
        {
            Executor.Execute($"EXECUTE InsertIntoCaseAndSummery @caseName = '{@case.CaseName}', @Clientid = {@case.ClientPerson.Id}, @Lawyerid = {@case.LawyerPerson.Id}, @secretary = {@case.SecretaryPerson.Id}, @caseKindId = {(int)@case.CaseKind}, @Summery = '{@case.Summery}'");
        }
        /// <summary>
        /// gets all cases with the specifik Id
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public List<Case> GetCases(string caseId)
        {
            List<Case> cases = new List<Case>();
            DataSet ds = Executor.Execute($"EXECUTE ClientData @clientId = {caseId}");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Person clientPerson = new Person(dr.Field<string>("ClientPerson"), RoleKind.Client);
                Person lawyerPerson = new Person(dr.Field<string>("LawyerPerson"), RoleKind.Lawyer);
                Person secretaryPerson = new Person(dr.Field<string>("SecretaryPerson"), RoleKind.Secretary);
                Case tempCase = new Case(dr.Field<string>("CaseName"), (CaseKind)Enum.Parse(typeof(CaseKind), dr.Field<string>("CaseKind")), secretaryPerson, lawyerPerson, clientPerson, dr.Field<int>("caseId"));
                cases.Add(tempCase);
            }
            return cases;
        }
        #endregion
    }
}
