using Advantage.Data.Provider;
using MedisoftFhirAgent.Controllers;
using MedisoftFhirAgent.DAL.Configuration;
using MedisoftFhirAgent.DAL.Entities;
using MedisoftFhirAgent.DAL.Entities.Scheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedisoftFhirAgent.DatabaseContexts
{
    class AdvantageContext
    {

        AdsCommand cmd;
        private LoggingController _lgc;
        public AdvantageContext()
        {
            _lgc = new LoggingController();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        }

        public AdsDataReader ExecuteQuery(string commandText)
        {
            AdsDataReader reader = null;

            using (AdsConnection conn = new AdsConnection("Data Source=C:\\MediData\\Tutor\\mwddf.add;User ID=user;Password=password;ServerType=REMOTE;Max Pool Size=50;Min Pool Size = 5; Pooling = True; "))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = commandText;
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return reader;
                    }

                    conn.Close();

                }
                catch (AdsException e)
                {
                    _lgc.Log("Medisoft Database exception", e.Message);
                }


            }
            return reader;


        }

        public bool logFailedRecords(string migId, string type, string message)
        {
            string query = "INSERT INTO MWMIG_ERR ([Migration ID], [Entity Type], [Message]) VALUES ('"+migId+"','"+type+"','"+message+"')";
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();
                    return true;

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Log_data_scheduler_", ex.Message);
                    return false;
                }
            }
        }

        public List<Patient> Patients()
        {
            try
            {
                string query = "select trim([chart number]) ChartNumber, trim([last Name]) lastName,trim([first Name]) firstName,"
                    + " trim([middle name]) middleName,trim([middle initial]) as prefix, trim(sex) sex ,trim(Suffix) suffix,trim(race) race ,trim(Ethnicity) Ethnicity,"
                    + " trim(Language) Language,trim([street 1])address1,trim([street 2]) address2,	trim(city) city,trim(state) state,"
                    + " trim([zip code]) zip, [phone 1] hPhone,[phone 2] wPhone,[phone 3] mobile, [phone 4] fax,[phone 5] altPhone,"
                    + " [date of Birth] dob, country,trim(email) email,trim([social security number]) ssn , "
                    + " trim([Assigned provider]) provider, [Inactive] inactive from MWPAT_TAR WHERE MWPAT_TAR.[Migration Status] = 'NM'";
                AdsDataReader reader = null;

                using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
                {
                    try

                    {
                        conn.Open();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = query;
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Patient> patients = new List<Patient>();
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    patients.Add(new Patient
                                    {
                                        Identifier = reader["ChartNumber"].ToString(),
                                        firstName = reader["firstName"].ToString(),
                                        lastName = reader["lastName"].ToString(),
                                        prefix = reader["prefix"].ToString(),
                                        birthDate = reader["dob"].ToString(),
                                        birthPlace = reader["city"].ToString(),
                                        citizenshipCode = reader["ssn"].ToString(),
                                        gender = reader["sex"].ToString(),
                                        address = new PatientAddress {
                                            streetName = reader["address1"].ToString(),
                                            streetNo = reader["address2"].ToString(),
                                            appartmentNo="",
                                            postalCode=reader["zip"].ToString(),
                                            city=reader["city"].ToString(),
                                            country=reader["country"].ToString(),
                                            type="",


                                        }
                                    });


                                }
                                conn.Close();
                                return patients;

                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (AdsException ex)
                    {
                        _lgc.Log("Medisoft Database exception", ex.Message);
                    }
                }
            }
            catch (AdsException ex)
            {
                _lgc.Log("Medisoft Database exception", ex.Message);

            }
            return null;
        }

        public List<Patient> DeletedPatients()
        {
            try
            {
                string query = "select trim([chart number]) ChartNumber, trim([last Name]) lastName,trim([first Name]) firstName,"
                    + " trim([middle name]) middleName,trim([middle initial]) as prefix, trim(sex) sex ,trim(Suffix) suffix,trim(race) race ,trim(Ethnicity) Ethnicity,"
                    + " trim(Language) Language,trim([street 1])address1,trim([street 2]) address2,	trim(city) city,trim(state) state,"
                    + " trim([zip code]) zip, [phone 1] hPhone,[phone 2] wPhone,[phone 3] mobile, [phone 4] fax,[phone 5] altPhone,"
                    + " [date of Birth] dob, country,trim(email) email,trim([social security number]) ssn , "
                    + " trim([Assigned provider]) provider, [Inactive] inactive from MWPAT_TAR WHERE MWPAT_TAR.[Deleted] = 1";
                AdsDataReader reader = null;

                using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
                {
                    try

                    {
                        conn.Open();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = query;
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Patient> patients = new List<Patient>();
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    patients.Add(new Patient
                                    {
                                        Identifier = reader["ChartNumber"].ToString(),
                                        firstName = reader["firstName"].ToString(),
                                        lastName = reader["lastName"].ToString(),
                                        prefix = reader["prefix"].ToString(),
                                        birthDate = reader["dob"].ToString(),
                                        birthPlace = reader["city"].ToString(),
                                        citizenshipCode = reader["ssn"].ToString(),
                                        gender = reader["sex"].ToString(),
                                        address = new PatientAddress
                                        {
                                            streetName = reader["address1"].ToString(),
                                            streetNo = reader["address2"].ToString(),
                                            appartmentNo = "",
                                            postalCode = reader["zip"].ToString(),
                                            city = reader["city"].ToString(),
                                            country = reader["country"].ToString(),
                                            type = "",


                                        }
                                    });


                                }
                                conn.Close();
                                return patients;

                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (AdsException ex)
                    {
                        _lgc.Log("Medisoft Database GetDelete exception", ex.Message);
                    }
                }
            }
            catch (AdsException ex)
            {
                _lgc.Log("Medisoft Database exception", ex.Message);

            }
            return null;
        }
        public List<Patient> getSecheduledPatients()
        {
            try
            {
                string query = "select trim([chart number]) ChartNumber, trim([last Name]) lastName,trim([first Name]) firstName,"
                    + " trim([middle name]) middleName,trim([middle initial]) as prefix, trim(sex) sex ,trim(Suffix) suffix,trim(race) race ,trim(Ethnicity) Ethnicity,"
                    + " trim(Language) Language,trim([street 1])address1,trim([street 2]) address2,	trim(city) city,trim(state) state,"
                    + " trim([zip code]) zip, [phone 1] hPhone,[phone 2] wPhone,[phone 3] mobile, [phone 4] fax,[phone 5] altPhone,"
                    + " [date of Birth] dob, country,trim(email) email,trim([social security number]) ssn , "
                    + " trim([Assigned provider]) provider, [Inactive] inactive from MWPAT WHERE TIMESTAMPDIFF( SQL_TSI_SECOND, MWPAT.[CreatedAt], now() ) <= 60";
                AdsDataReader reader = null;

                using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
                {
                    try

                    {
                        conn.Open();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = query;
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            Debug.WriteLine("has");
                            List<Patient> patients = new List<Patient>();
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    Debug.WriteLine(reader["ChartNumber"].ToString());
                                    patients.Add(new Patient
                                    {
                                        Identifier = reader["ChartNumber"].ToString(),
                                        firstName = reader["firstName"].ToString(),
                                        lastName = reader["lastName"].ToString(),
                                        prefix = reader["prefix"].ToString(),
                                        birthDate = reader["dob"].ToString(),
                                        birthPlace = reader["city"].ToString(),
                                        citizenshipCode = reader["ssn"].ToString(),
                                        gender = reader["sex"].ToString(),
                                        address = new PatientAddress
                                        {
                                            streetName = reader["address1"].ToString(),
                                            streetNo = reader["address2"].ToString(),
                                            appartmentNo = "",
                                            postalCode = reader["zip"].ToString(),
                                            city = reader["city"].ToString(),
                                            country = reader["country"].ToString(),
                                            type = "",


                                        }
                                    });


                                }
                                conn.Close();
                                return patients;

                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (AdsException ex)
                    {
                        _lgc.Log("Medisoft Database exception", ex.Message);
                    }
                }
            }
            catch (AdsException ex)
            {
                _lgc.Log("Medisoft Database exception", ex.Message);

            }
            return null;
        }


        public List<Patient> UpdatedPatients()
        {
            try
            {
                string query = "select trim([chart number]) ChartNumber, trim([last Name]) lastName,trim([first Name]) firstName,"
                    + " trim([middle name]) middleName,trim(sex) sex ,trim(Suffix) suffix,trim(race) race ,trim(Ethnicity) Ethnicity,"
                    + " trim(Language) Language,trim([street 1])address1,trim([street 2]) address2,	trim(city) city,trim(state) state,"
                    + " trim([zip code]) zip, [phone 1] hPhone,[phone 2] wPhone,[phone 3] mobile, [phone 4] fax,[phone 5] altPhone,"
                    + " [date of Birth] dob, country, trim([middle initial]) prefix, trim(email) email,trim([social security number]) ssn , "
                    + " trim([Assigned provider]) provider, [Inactive] inactive from MWPAT WHERE TIMESTAMPDIFF( SQL_TSI_SECOND, MWPAT.[CreatedAt], MWPAT.[Date Modified] ) > 0 AND TIMESTAMPDIFF( SQL_TSI_SECOND, MWPAT.[Date Modified], now() ) <= 60";
                AdsDataReader reader = null;

                using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
                {
                    try

                    {
                        conn.Open();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = query;
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Patient> patients = new List<Patient>();
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    patients.Add(new Patient
                                    {
                                        Identifier = reader["ChartNumber"].ToString(),
                                        firstName = reader["firstName"].ToString(),
                                        lastName = reader["lastName"].ToString(),
                                        prefix = reader["prefix"].ToString(),
                                        birthDate = reader["dob"].ToString(),
                                        birthPlace = reader["state"].ToString(),
                                        citizenshipCode = reader["ssn"].ToString(),
                                        gender = reader["sex"].ToString(),
                                        address = new PatientAddress
                                        {
                                            streetName = reader["address1"].ToString(),
                                            streetNo = reader["address2"].ToString(),
                                            appartmentNo = "",
                                            postalCode = reader["zip"].ToString(),
                                            city = reader["city"].ToString(),
                                            country = reader["country"].ToString(),
                                            type = "",


                                        }
                                    });


                                }
                                conn.Close();
                                return patients;

                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (AdsException ex)
                    {
                        _lgc.Log("Medisoft Database exception", ex.Message);
                    }
                }
            }
            catch (AdsException ex)
            {
                _lgc.Log("Medisoft Database exception", ex.Message);

            }
            return null;
        }
        public bool setPatientUpdatedDataMigrationStatus(List<Patient> listPatient)
        {
            List<string> Identierfiers = new List<string>();
            foreach (var pat in listPatient)
            {
                Identierfiers.Add("'" + pat.Identifier + "'");
            }
            string joined = string.Join(",", Identierfiers);
            Debug.WriteLine(joined);

            string query = "UPDATE MWMIG_UPD SET MWMIG_UPD.[Status] = 1 FROM MWMIG_UPD WHERE MWMIG_UPD.[KeyValue] IN (" + joined + ")";
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                    return true;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Data Migration Status(Patients)", ex.Message);
                }
            }

            return false;
        }

        public bool updateDeletedRecordsMigrated(List<Patient> listPatient)
        {
            List<string> Identierfiers = new List<string>();
            foreach (var pat in listPatient)
            {
                Identierfiers.Add("'" + pat.Identifier + "'");
            }
            string joined = string.Join(",", Identierfiers);
            Debug.WriteLine(joined);

            string query = "UPDATE MWPAT_TAR SET MWPAT_TAR.[Deleted] = 2 FROM MWPAT_TAR WHERE MWPAT_TAR.[Chart Number] IN (" + joined + ")";
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                    return true;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Update Deleted Migration Record Status", ex.Message);
                }
            }

            return false;
        }

        public bool setPatientDataMigrationStatus(List<Patient> listPatient)
        {
            List<string> Identierfiers = new List<string>();
            foreach(var pat in listPatient)
            {
                Identierfiers.Add("'" + pat.Identifier + "'");
            }
            string joined = string.Join(",", Identierfiers);
            Debug.WriteLine(joined);

            string query = "UPDATE MWPAT_TAR SET MWPAT_TAR.[Migration Status] = 'M' FROM MWPAT_TAR WHERE MWPAT_TAR.[Chart Number] IN (" + joined+")";
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                    return true;
                }
                catch(AdsException ex)
                {
                    _lgc.Log("Data Migration Status(Patients)", ex.Message);
                }
               }

                return false;
        }

        public bool insertIntoPatients(List<Patient> listPatients)
        {
            List<string> inst = new List<string>();
            string joined = "";
            foreach(var pt in listPatients)
            {
                List<string> eachPat = new List<string>();
                eachPat.Add("\'" + pt.Identifier + "\'");
                eachPat.Add("\'" +pt.firstName+ "\'");
                eachPat.Add("\'"+pt.lastName + "\'");
                eachPat.Add("\'" + pt.prefix + "\'");
                eachPat.Add("\'" + pt.gender + "\'");
                eachPat.Add("\'" + pt.birthDate + "\'");
                eachPat.Add("\'" + pt.birthPlace + "\'");
                eachPat.Add("\'" + pt.citizenshipCode + "\'");
                eachPat.Add("\'" + pt.address.appartmentNo + " "+pt.address.streetNo + " "+pt.address.streetName + "\'");
                eachPat.Add("\'" + pt.address.city + "\'");
                eachPat.Add("\'" + pt.address.type + "\'");
                eachPat.Add("\'" + pt.address.country + "\'");
                eachPat.Add("\'" + "00/00/0000 00:00:00" + "\'");
                joined = string.Join(",", eachPat);
                inst.Add("(" + joined + ")");
            }
            string result = string.Join(",",inst);
            Debug.WriteLine(result);

            string query = "INSERT INTO MWPAT ([Chart Number], [First Name], [Last Name], [Middle Initial], [Sex], [Date of Birth], [State], [Social Security Number], [Street 1], [City], [Patient Type], [Country], [CreatedAt]) VALUES " + result;
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                    return true;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Data Migration Into PAtient_", ex.Message);
                }
            }
            return true;

        }

        public bool deletePatients(List<Patient> listPatients)
        {
            string inst = "";
            string joined = "";
            List<string> eachPat = new List<string>();
            foreach (var pt in listPatients)
            {
                eachPat.Add("\'" + pt.Identifier + "\'");
            
           
            }
            joined = string.Join(",", eachPat);
            inst ="(" + joined + ")";

            string query = "Delete FROM MWPAT WHERE [Chart Number] IN " + inst;
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                    return true;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Data Deletion FROM MWPAT ", ex.Message);
                }
            }
            return true;

        }
        public List<Duration> getLogDurations()
        {
            string query = "SELECT [Id], [start], [status] FROM MWSCH WHERE MWSCH.[status] = 0 LIMIT 2";
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection("Data Source=C:\\MediData\\Tutor\\mwddf.add;User ID=user;Password=password;ServerType=REMOTE;Max Pool Size=50;Min Pool Size = 5; Pooling = True; "))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<Duration> dur = new List<Duration>();
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                dur.Add(new Duration
                                {
                                    Id = int.Parse(reader["Id"].ToString()),
                                    start = reader["start"].ToString(),
                                    status = int.Parse(reader["status"].ToString())

                                });


                            }
                            return dur;

                        }
                    }
                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Log_data_duration_scheduler_", ex.Message);
                }
                return null;
            }
        }

        public void logSchedulerService(DateTime dt_start, DateTime dt_end)
        {
            string query = "INSERT INTO MWSCH ([start]) VALUES (now())";
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection("Data Source=C:\\MediData\\Tutor\\mwddf.add;User ID=user;Password=password;ServerType=REMOTE;Max Pool Size=50;Min Pool Size = 5; Pooling = True; "))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Log_data_scheduler_", ex.Message);
                }
            }
        }

        public void createInboundRecord(List<Patient> listPatients)
        {
            List<string> inst = new List<string>();
            string joined = "";
            foreach (var pt in listPatients)
            {
                List<string> eachPat = new List<string>();
                eachPat.Add("\'" + pt.Identifier + "\'");
                eachPat.Add("\'" +"Patient"+ "\'");
                joined = string.Join(",", eachPat);
                inst.Add("(" + joined + ")");
            }
            string result = string.Join(",", inst);

            string query = "INSERT INTO MWMIG_INS ([KeyValue], [TableType]) VALUES " + result;
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Log_data_scheduler_", ex.Message);
                }
            }
        }


        public bool isInboundReord(string identifier, string type)
        {
            string query = "Select ta.[KeyValue] From MWMIG_INS ta WHERE ta.[KeyValue] = '" + identifier + "' AND ta.[TableType]='"+type+"'";
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }

                    conn.Close();

                    return false;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Merge_data_scheduler_", ex.Message);
                    return false;
                }
            }
        }

        public void MergeYeatsRecordsUpdatedByMedisoft()
        {
            string query = "DELETE FROM MWMIG_INS Where [KeyValue] IN (SELECT tn.[KeyValue] FROM MWMIG_INS tn INNER JOIN MWPAT tp ON tn.[KeyValue] = tp.[Chart Number] WHERE tp.[Date Modified] IS NOT NULL)";
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Merge_updated_by_medisoft_", ex.Message);
                }
            }
        }

        public void trackNewPatients()
        {
            string query1 = "MERGE MWPAT_TAR AS ta "
                         + "USING (SELECT * FROM MWPAT tw WHERE tw.[Chart Number] NOT IN (SELECT [KeyValue] FROM MWMIG_INS tn)) AS tb "
                         + "ON(ta.[Chart Number] = tb.[Chart Number])   "
                         + "WHEN NOT MATCHED THEN "
                         + "INSERT ([Chart Number], [First Name], [Last Name], [Middle Initial], [Sex], [Date of Birth], [State], [Social Security Number], [Street 1], [City], [Patient Type], [Country], [Migration Status]) VALUES (tb.[Chart Number], tb.[First Name], tb.[Last Name], tb.[Middle Initial], tb.[Sex], tb.[Date of Birth], tb.[State], tb.[Social Security Number], tb.[Street 1], tb.[City], tb.[Patient Type], tb.[Country], 'NM' )";



            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query1;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Merge_data_scheduler_", ex.Message);
                }
            }

        }
        public void trackUpdatedPatients()
        {
            string query = "MERGE MWPAT_TAR AS ta "
                        + "USING MWPAT AS tb "
                        + "ON(ta.[Chart Number] = tb.[Chart Number] AND ta.[First Name] <> tb.[First Name]) "
                        + "WHEN MATCHED  "
                        + "THEN UPDATE SET ta.[First Name] = tb.[First Name] , ta.[Last Name] = tb.[Last Name] , ta.[Middle Initial] = tb.[Middle Initial], ta.[Sex] = tb.[Sex], ta.[Date of Birth] = tb.[Date of Birth], ta.[State] = tb.[State], ta.[Social Security Number] = tb.[Social Security Number], ta.[Street 1] = tb.[Street 1], ta.[City] = tb.[City], ta.[Patient Type] =  tb.[Patient Type], ta.[Country] = tb.[Country],  ta.[Migration Status] = 'NM' ";

            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Merge_data_scheduler_", ex.Message);
                }
            }


        }

           public void MergeDeletedRecords()
           {
            string query = "UPDATE MWPAT_TAR SET MWPAT_TAR.[Deleted] = 1 FROM MWPAT_TAR ta "+
                            "WHERE  ta.[Deleted] = 0 AND ta.[Chart Number] not in "+ 
                                    "(SELECT c.[Chart Number] "+
                                       "FROM MWPAT c ); ";
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Deleted_records_exception_", ex.Message);
                }
            }


        }
        public void mergeMedisoft()
        {
            this.MergeYeatsRecordsUpdatedByMedisoft();
            this.trackNewPatients();
            this.trackUpdatedPatients();
            this.MergeDeletedRecords();
        }

        public bool findMigratedPatient(Patient obj)
        {
            string query = "Select ta.[Chart Number] From MWPAT_TAR ta WHERE ta.[Chart Number] = '" + obj.Identifier +"' AND ta.[Migration Status]='M'";
            Debug.WriteLine(query);
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }

                    conn.Close();

                    return false ;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Merge_data_scheduler_", ex.Message);
                    return false;
                }
            }
        }
        public bool findPatient(Patient obj)
        {
            string query = "Select ta.[Chart Number] From MWPAT ta WHERE ta.[Chart Number] = '" + obj.Identifier + "'";
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }

                    conn.Close();
                    return false;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Patient_already_In_MEdisoft_", ex.Message);
                    return false;
                }
            }
        }

        public void updatePatient(Patient obj)
        {

            string query = "UPDATE MWPAT SET [First Name] = '" + obj.firstName + "', [Last Name] = '" + obj.lastName + "', [Middle Initial] = '" + obj.prefix + "', [Date of Birth] = '" + obj.birthDate + "', [Social security number] = '" + obj.citizenshipCode + "', [Sex] = '" + obj.gender + "', [Street 1] = '" + obj.address.appartmentNo + " " + obj.address.streetNo + " " + obj.address.streetName + "', [Zip Code] = '" + obj.address.postalCode + "', [City] = '" + obj.address.city + "' ,[Country] = '" + obj.address.country+"'"  + " FROM MWPAT tb WHERE tb.[Chart Number] = '"+obj.Identifier+"'";
            AdsDataReader reader = null;
            Debug.WriteLine(query);
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                }
                catch (AdsException ex)
                {
                    _lgc.Log("update_patient_", ex.Message);
                }
            }
        }
        public bool setMigrationConplete(Patient obj)
        {
            string query = "MERGE MWPAT_TAR ta "
                          + "ON(ta.[Chart Number] = '"+obj.Identifier+"') " 
                          + "WHEN MATCHED THEN "
                          + "UPDATE SET ta.[Migration Status] = 'C'";
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();
                    
                    conn.Close();
                    return true;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Merge_data_scheduler_", ex.Message);
                    return false;
                }
            }
        }


        public bool setMigrationFailed(Patient obj)
        {
            string query = "MERGE MWPAT_TAR ta "
                          + "ON(ta.[Chart Number] = '" + obj.Identifier + "') "
                          + "WHEN MATCHED THEN "
                          + "UPDATE SET ta.[Migration Status] = 'F'";
            AdsDataReader reader = null;
            using (AdsConnection conn = new AdsConnection(DatabaseSettings.DatabaseConnection))
            {
                try

                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();

                    conn.Close();
                    return true;
                }
                catch (AdsException ex)
                {
                    _lgc.Log("Merge_data_scheduler_", ex.Message);
                    return false;
                }
            }
        }




    }

}
