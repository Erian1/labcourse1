using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public DevelopersController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]

        public JsonResult Get()
        {
            String query = @"
                select DevelopersId, DevelopersName, DevelopersPosition, convert(varchar(10),DateOfJoining,120)as DateOfJoining
                ,PhotoFileName
                from dbo.Developers
                
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LabProject");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Developers dev)
        {
            string query = @"
                    insert into dbo.Developers 
                    (DevelopersName,DevelopersPosition,DateOfJoining,PhotoFileName)
                    values 
                    (
                    '" + dev.DevelopersName + @"'
                    ,'" + dev.DevelopersPosition + @"'
                    ,'" + dev.DateOfJoining + @"'
                    ,'" + dev.PhotoFileName + @"'
                    )
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LabProject");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]

        public JsonResult Put(Developers dev)
        {
            string query = @"
            update dbo.Developers set 
            DevelopersName =  '" + dev.DevelopersName + @"'
            ,DevelopersPosition =  '" + dev.DevelopersPosition + @"'
            ,DateOfJoining =  '" + dev.DateOfJoining + @"'
          
            where DevelopersId = " + dev.DevelopersId + @"
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LabProject");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");

        }

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            String query = @"
            Delete from dbo.Developers
            where DevelopersId = " + id + @"
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LabProject");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");

        }

        [Route("SaveFile")]

        [HttpPost]

        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("Unknown.jpg");
            }
        }



            [Route("GetAllAdminNames")]
            public JsonResult GetAllAdminNames()
            {
                string query = @"
                    select AdminName from dbo.Admin
                    ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("LabProject");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader); ;

                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult(table);
            }

    }
}
    


