using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private string playerName = "Test_0";

    [Header("Camera")]
    [SerializeField] private Camera playCamera;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("[PlayerSpawner] Player Prefab이 할당되지 않았습니다.");
            return;
        }

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion spawnRot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        GameObject player = Instantiate(playerPrefab, spawnPos, spawnRot);
        player.name = playerName;

        SetupCamera(player.transform);
        SetupPlayerMovement(player);
    }

    private void SetupCamera(Transform target)
    {
        if (playCamera == null)
        {
            playCamera = Camera.main;
        }

        if (playCamera == null)
        {
            Debug.LogWarning("[PlayerSpawner] 연결할 Camera를 찾을 수 없습니다.");
            return;
        }

        CinemachineThirdPersonCameraController camController = playCamera.GetComponent<CinemachineThirdPersonCameraController>();
        if (camController == null)
        {
            camController = playCamera.gameObject.AddComponent<CinemachineThirdPersonCameraController>();
        }

        camController.SetTarget(target);
    }

    private void SetupPlayerMovement(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement == null)
        {
            Debug.LogWarning("[PlayerSpawner] PlayerMovement를 찾을 수 없습니다.");
            return;
        }

        if (playCamera != null)
        {
            movement.SetCamera(playCamera.transform);
        }
    }
}
