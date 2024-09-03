using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    public float zVel = 0.7f; // ��ü�� ��,�� �ӵ� ������ �ּ� �ӵ�
    public float xVel = 0.7f; // ��ü�� ����,������ �ӵ� ������ �ּ� �ӵ�
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ��ü�� ������ٵ� �ӵ�(�۷ι� Vector3 ����)�� ���� �� ���� ���·� ��ȯ
        Vector3 velocity = transform.InverseTransformDirection(rb.velocity);

        if (isDead)
        {
            anim.SetBool("isDead", true);
        }
        else
        {
            // �ӵ� ���� ���� �ּڰ����� ������ �ִϸ��̼� ����
            if (velocity.z > zVel || velocity.z < -zVel || velocity.x > xVel || velocity.x < -xVel)
            {
                anim.SetBool("isMove", true);
                anim.SetFloat("xDir", velocity.z);
                anim.SetFloat("yDir", velocity.x * 0.5f);
            }
            else
            {
                anim.SetBool("isMove", false);
                anim.SetFloat("xDir", 0);
                anim.SetFloat("yDir", 0);
            }
        }
    }

    public void SetTrigger(string trgName)
    {
        anim.SetTrigger(trgName);
    }

    public void isDie(bool die)
    {
        isDead = die;
    }
}
