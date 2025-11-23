using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using AppMvc.SeidoHelpers;
using Models.DTO;
using Models.Interfaces;

namespace AppMvc.Models
{
	public class EditGroupViewModel
    {
        [BindProperty]
        public MusicGroupIM MusicGroupInput { get; set; }

        //I also use BindProperty to keep between several posts, bound to hidden <input> field
        [BindProperty]
        public string PageHeader { get; set; }

        //Used to populate the dropdown select
        //Notice how it will be populate every time the class is instansiated, i.e. before every get and post
        public List<SelectListItem> GenreItems { set; get; } = new List<SelectListItem>().PopulateSelectList<MusicGenre>();

        //For Validation
        public ModelValidationResult ValidationResult { get; set; } = new ModelValidationResult(false, null, null);

        #region Input Model
        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        //These classes are in center of ModelBinding and Validation
        public enum StatusIM { Unknown, Unchanged, Inserted, Modified, Deleted }
        public class ArtistIM
        {
            public StatusIM StatusIM { get; set; }

            public Guid ArtistId { get; set; }

            [Required(ErrorMessage = "You must provide a first name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "You must provide a last name")]
            public string LastName { get; set; }

            //This is because I want to confirm modifications in PostEditAlbum
            [Required(ErrorMessage = "You must provide a first name")]
            public string editFirstName { get; set; }

            [Required(ErrorMessage = "You must provide a last name")]
            public string editLastName { get; set; }

            public ArtistIM() { }
            public ArtistIM(ArtistIM original)
            {
                StatusIM = original.StatusIM;
                ArtistId = original.ArtistId;
                FirstName = original.FirstName;
                LastName = original.LastName;

                editFirstName = original.editFirstName;
                editLastName = original.editLastName;
            }
            public ArtistIM(IArtist model)
            {
                StatusIM = StatusIM.Unchanged;
                ArtistId = model.ArtistId;
                FirstName = editFirstName = model.FirstName;
                LastName = editLastName = model.LastName;
            }

                        //to update the model in database
            public IArtist UpdateModel(IArtist model)
            {
                model.ArtistId = this.ArtistId;
                model.FirstName = this.FirstName;
                model.LastName = this.LastName;
                return model;
            }

            //to create new artist in the database
            public ArtistCUdto CreateCUdto () => new ArtistCUdto(){

                ArtistId = null,
                FirstName = this.FirstName,
                LastName = this.LastName
            };
        }
        public class AlbumIM
        {
            public StatusIM StatusIM { get; set; }

            public Guid AlbumId { get; set; }

            [Required(ErrorMessage = "You must enter an album name")]
            public string AlbumName { get; set; }

            [Range(1900, 2024, ErrorMessage = "You must provide a year between 1900 and 2024")]
            public int ReleaseYear { get; set; }


            [Required(ErrorMessage = "You must enter an album name")]
            public string editAlbumName { get; set; }

            [Range(1900, 2024, ErrorMessage = "You must provide a year between 1900 and 2024")]
            public int editReleaseYear { get; set; }

            public AlbumIM() { }
            public AlbumIM(AlbumIM original)
            {
                StatusIM = original.StatusIM;
                AlbumId = original.AlbumId;
                AlbumName = original.AlbumName;
                ReleaseYear = original.ReleaseYear;


                editAlbumName = original.editAlbumName;
                editReleaseYear = original.editReleaseYear;
            }
            public AlbumIM(IAlbum model)
            {
                StatusIM = StatusIM.Unchanged;
                AlbumId = model.AlbumId;
                AlbumName = editAlbumName = model.Name;
                ReleaseYear = editReleaseYear = model.ReleaseYear;
            }

                        //to update the model in database
            public IAlbum UpdateModel(IAlbum model)
            {
                model.AlbumId = this.AlbumId;
                model.Name = this.AlbumName;
                model.ReleaseYear = this.ReleaseYear;
                return model;
            }

            //to create new album in the database
            public AlbumCUdto CreateCUdto () => new AlbumCUdto(){

                AlbumId = null,
                Name = this.AlbumName,
                ReleaseYear = this.ReleaseYear
            };
        }
        public class MusicGroupIM
        {
            public StatusIM StatusIM { get; set; }

            public Guid MusicGroupId { get; set; }

            [Required(ErrorMessage = "You must provide a group name")]
            public string Name { get; set; }

            [Range(1900, 2024, ErrorMessage = "You must provide a year between 1900 and 2024")]
            public int EstablishedYear { get; set; }

            //Made nullable and required to force user to make an active selection when creating new group
            [Required(ErrorMessage = "You must select a music genre")]
            public MusicGenre? Genre { get; set; }

            public List<AlbumIM> Albums { get; set; } = new List<AlbumIM>();
            public List<ArtistIM> Artists { get; set; } = new List<ArtistIM>();

            public MusicGroupIM() { }
            public MusicGroupIM(IMusicGroup model)
            {
                StatusIM = StatusIM.Unchanged;
                MusicGroupId = model.MusicGroupId;
                Name = model.Name;
                EstablishedYear = model.EstablishedYear;
                Genre = model.Genre;

                Albums = model.Albums?.Select(m => new AlbumIM(m)).ToList();
                Artists = model.Artists?.Select(m => new ArtistIM(m)).ToList();
            }


            //to update the model in database
            public IMusicGroup UpdateModel(IMusicGroup model)
            {
                model.Name = this.Name;
                model.EstablishedYear = this.EstablishedYear;
                model.Genre = this.Genre.Value;
                return model;
            }

            //to create new music group in the database
            public MusicGroupCUdto CreateCUdto () => new (){
                
                MusicGroupId = null,
                Name = this.Name,
                EstablishedYear = this.EstablishedYear,
                Genre = this.Genre.Value
            };

            //to allow a new album being specified and bound in the form
            public AlbumIM NewAlbum { get; set; } = new AlbumIM();

            //to allow a new album being specified and bound in the form
            public ArtistIM NewArtist { get; set; } = new ArtistIM();
        }
        #endregion
    }
}

