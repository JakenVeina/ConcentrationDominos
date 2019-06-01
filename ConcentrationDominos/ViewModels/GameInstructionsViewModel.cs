using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Windows.Input;

using ConcentrationDominos.Gameplay;
using ConcentrationDominos.Models;

namespace ConcentrationDominos.ViewModels
{
    public interface IGameInstructionsViewModel
        : IDisposable
    {
        IReadOnlyList<string> InstructionLines { get; }

        IActionCommand ConfirmCommand { get; }
    }

    public class GameInstructionsViewModel
        : ViewModelBase,
            IGameInstructionsViewModel
    {
        public GameInstructionsViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _subscriptions = new CompositeDisposable();

            ConfirmCommand = new ActionCommand(
                    execute: () => _navigationService.NavigateTo(null),
                    canExecute: navigationService.CanNavigateTo)
                .DisposeWith(_subscriptions);
        }

        public IReadOnlyList<string> InstructionLines
            => GameInstructionsModel.InstructionLines;

        public IActionCommand ConfirmCommand { get; }

        public void Dispose()
            => _subscriptions.Dispose();

        private readonly INavigationService _navigationService;

        private readonly CompositeDisposable _subscriptions;
    }
}
