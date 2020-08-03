using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.mvc.demo.DataAccess.Entity
{
    [Table("girl")]
    public class Girl
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Column("girl_id")]
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Column("girl_name")]
        public string Name { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        [Column("girl_age")]
        public int Age { get; set; }

        /// <summary>
        /// PhoneNum
        /// </summary>
        [Column("girl_phonenumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// loverName
        /// </summary>
        [Column("girl_belovedname")]
        public string BelovedName { get; set; }
    }
}
