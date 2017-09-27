namespace Weather.Services
{

    public interface IPhotoTaker
    {
        bool IsThereAnAppToTakePictures();
        void CreateDirectoryForPictures();
    }
}