using UnityEngine;

public class MeteorMover : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1f; // 천천히 내려오도록 기본값 조정
    public Vector3 direction = Vector3.down; // 아래로 향함
    
    [Header("Impact Settings")]
    public float groundY = 0f; // 땅의 높이
    private bool hasImpacted = false;
    
    public delegate void MeteorImpactAction();
    public static event MeteorImpactAction OnImpact; // 게임 매니저 등에서 구독 가능

    [Header("Rotation")]
    [Tooltip("운석 메쉬 오브젝트를 여기에 넣으면 꼬리 방향은 고정된 채 몸체만 회전합니다.")]
    public Transform visualBody;
    public Vector3 rotationSpeed = new Vector3(30f, 60f, 20f); // 회전도 좀 더 천천히

    void Update()
    {
        if (hasImpacted) return;

        // 1. 전체 이동
        transform.position += direction * speed * Time.deltaTime;
        
        // 2. 몸체 회전
        if (visualBody != null)
            visualBody.Rotate(rotationSpeed * Time.deltaTime);
        else
            transform.Rotate(rotationSpeed * Time.deltaTime);
        
        // 3. 땅 도달 체크
        if (transform.position.y <= groundY)
        {
            HandleImpact();
        }
    }

    private void HandleImpact()
    {
        hasImpacted = true;
        
        // 위치 고정
        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;

        // 이벤트 발생 (게임 종료 알림)
        OnImpact?.Invoke();
        
        Debug.Log("☄️ 운석이 지면에 도달했습니다! 게임 종료 이벤트를 발생시킵니다.");
    }
}
