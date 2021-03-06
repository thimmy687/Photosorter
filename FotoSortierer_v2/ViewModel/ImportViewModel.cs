﻿using System.Windows.Input;
using FotoSortierer_v2.Helper.Adapter;
using FotoSortierer_v2.Helper.Adapter.Interfaces;
using FotoSortierer_v2.Services.Interfaces;
using FotoSortierer_v2.ViewModel.Interfaces;
using Model;
using Model.Interfaces;
using MVVM;
using MVVM.Messenger;

namespace FotoSortierer_v2.ViewModel
{
    /// <inheritdoc cref="IImportViewModel" />
    public class ImportViewModel : ViewModelBase<ImportModel>, IImportViewModel
    {
        private readonly IMessenger _messenger;
        private readonly IPhotoService _photoService;
        private readonly ICameraService _cameraService;
        private IObservableCollectionAdapter<ICameraViewModel> _cameras;
        private IObservableCollectionAdapter<IPhotoViewModel> _photos; // Todo: make collections readonly

        public ImportViewModel(IMessenger messenger, IPhotoService photoService, ICameraService cameraService)
        {
            _messenger = messenger;
            _photoService = photoService;
            _cameraService = cameraService;

            _photos = new ObservableCollectionAdapter<IPhotoViewModel>();
            _cameras = new ObservableCollectionAdapter<ICameraViewModel>();

            OkCommand = new RelayCommand(p => ExecuteOk());
            OpenCommand = new RelayCommand(p => ExecuteOpen());
        }

        private void ExecuteOk()
        {
            _messenger.Send(1, "ImportFinished");
        }

        private async void ExecuteOpen()
        {
            Photos.AddRange(await _photoService.GetPhotosAsync());
            Cameras.AddRange(_cameraService.GetCameras(_photos));
        }

        public ICommand OpenCommand { get; }
        public ICommand OkCommand { get; }

        /// <inheritdoc />
        public IObservableCollectionAdapter<ICameraViewModel> Cameras
        {
            get => _cameras;
            set
            {
                if (_cameras == value) return;
                _cameras = value;
                OnPropertyChanged();
            }
        }
        /// <inheritdoc />
        public IPhotoModel SelectedPhoto
        {
            get => Model.SelectedPhoto;
            set
            {
                if (Model.SelectedPhoto == value) return;
                Model.SelectedPhoto = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IObservableCollectionAdapter<IPhotoViewModel> Photos
        {
            get => _photos;
            set
            {
                if (_photos == value) return;
                _photos = value;
                OnPropertyChanged();
            }
        }
    }
}