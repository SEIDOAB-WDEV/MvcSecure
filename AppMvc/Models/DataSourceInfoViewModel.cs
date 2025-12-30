using System.ComponentModel.DataAnnotations;
using AppMvc.SeidoHelpers;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace AppMvc.Models
{
	public class DataSourceInfoViewModel
    {
        public GstUsrInfoAllDto Info { get; set; }
    }
}