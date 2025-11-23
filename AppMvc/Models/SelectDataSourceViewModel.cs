using Microsoft.AspNetCore.Mvc;

using Services;

namespace AppMvc.Models
{
	public class SelectDataSourceViewModel
    {
        //ModelBinding for Selections
        [BindProperty]
        public MusicDataSource SelectedDataSource { get; set; }
    }
}

