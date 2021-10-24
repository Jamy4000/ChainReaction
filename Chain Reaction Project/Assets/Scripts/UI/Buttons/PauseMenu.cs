using UnityEngine;

public class PauseMenu : AddListenerButton
{
    [SerializeField] private GameObject pauseMenu;

    protected override void OnClick() => pauseMenu.SetActive(!pauseMenu.activeSelf);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnClick();
    }
}
