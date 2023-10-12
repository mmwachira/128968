using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace SojaExiles

{
	public class opencloseDoor : MonoBehaviour
	{

		public Animator openandclose;
		public bool open;
		public Transform Player;

		public GameObject opendoor;
		public TMP_Text openDoor;

		void Start()
		{
			open = false;
		}


		void OnMouseOver()
		{
			{
				if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 2)
					{
						if (open == false)
						{
							opendoor.SetActive(true);
							openDoor.text = "Press [E] to open door";

							//if (Input.GetMouseButtonDown(0))
							if (Input.GetKeyDown("e"))
							{
								StartCoroutine(opening());
								//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
							}
						}
						else if (open == true)
						//{
							//if (open == true)
							{
								opendoor.SetActive(true);
								openDoor.text = "Press [E] to close door";

								//if (Input.GetMouseButtonDown(0))
								if (Input.GetKeyDown("e"))
								{
									StartCoroutine(closing());
								}
							}
					}

					if (dist > 2)
					{
						if(open == false || open == true)
						{
							opendoor.SetActive(false);
							openDoor.text = "";
						}
							
					}

						//else if (dist > 2 && open == true)
						//{
						//	openDoor.text = "";
						//}

						//}

					//}

					//if (dist > 2)
					//{
						//openDoor.text = "";
					//}
				}

			}

		}

		IEnumerator opening()
		{
			print("you are opening the door");
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			openandclose.Play("Closing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}