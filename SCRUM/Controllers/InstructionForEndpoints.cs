using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Controllers
{
    public class InstructionForEndpoints
    {
        /*
         * Tam gdzie plus (+) - brak uwzglednienia w wymaganiach, czyli kawal dobrej nikomu niepotrzebnej roboty, 
         * ktorej i tak nikt nigdy nie wykorzysta... Ale moze chociaz doceni? 
         * 
         * (:
         * 
         * TODO: signalR
         * 
         * git rebase origin/main
         * git push
         * 
        Wszystkie endpointy i co im dajemy:

        LOGOWANIE
        
            **** TESTOWO
            GET api/getUsers    - ()                    - zwraca liste uzytkownikow - tylko do testow
            ****
        
        +   POST    api/signup          - (Name: string)        - rejestracja wymagana, by sie zarejestrowac nalezy podac tylko nazwe.
            POST    api/signin          - (Name: string)        - logowanie z uzyciem nazwy uzytkownika
            GET     api/signout         - ()                    - wylogowanie, automatyczne usuniecie ciasteczka
            GET     api/getMyProfile    - ()                    - zwraca nazwe uzytkownika, jezeli jest zalogowany
        +   POST    api/role            - (model: RoleModel)    - dodaje role (uprawnienia do konkretnego projektu okreslonemu uzytkownikowi

        PROJEKT - musi byc utworzony pierwszy
            * Zabezpieczenia projektu:
            * Utworzenie i odczytanie   - niezabezpieczone  - tylko zalogowany (bez roznicy jako kto, bo i tak nie ma przydzielonej roli do wszystkich)
            * Edytowanie i usuniecie    - zabezpieczone     - tylko osoby upowaznione

        +   GET     api/projects        - ()                    - zwraca liste wszystkich projektow
        +   GET     api/projects/{tag}  - (tag: string)         - zwraca id projektu o danym tagu
        +   POST    api/projects        - (model: ProjectModel) - tworzy projekt    
        +   PUT     api/projects        - (model: ProjectModel) - edytuje projekt
        +   DELETE  api/projects/{id}   - ()                    - usuwa projekt


        PROPOZYCJE WYMAGAŃ - do konkretnego projektu

            GET     api/pendingrequirements/my/{projectId}          - ()                                - zwraca własne wymagania dla konkretnego projektu
            GET     api/pendingrequirements/{projectId}             - ()                                - zwraca wymagania dla konkretnego projektu 
            POST    api/pendingrequirements                         - (model: PendingRequirementModel)  - tworzy wymaganie do projektu 
            PUT     api/pendingrequirements                         - (model: PendingRequirementModel)  - edytuje wymaganie
            DELETE  api/pendingrequirements/{projectId}/{pendingId} - ()                                - usuwa wymaganie

        WYMAGANIA - do konkretnego projektu
            
            POST api/requirements/{pendingId}   - (model: RequirementModel)             - zatwierdzenie propozycji wymagania
            GET api/requirements/{projectId}    - ()                                    - zwraca wymagania dla konkretnego projektu 
            PUT api/requirements                - (model: RequirementModel)             - edytuje wymaganie - oprócz statusu
            PUT api/requirements/started        - (model: RequirementModel)             - edytuje wymaganie - oprócz statusu wymagania w toku
            PUT api/requirements/status         - (model: RequirementModel)             - edytuje status wymagania
            PUT api/requirements/order          - (modelList: List<RequirementModel>)   - edytuje order wymagania

        MODELE - używane przeze mnie:

            PendingRequirementModel:
                int ProjectId 
                string Title 
                string Description
                string Status ("Oczekuje", "Odrzucone")

            ProjectModel:
                string Title
                string Description
                string Tag

            Requirement:
                int ProjectId 
                string Title 
                string Description
                string Touchstone 
                int Priority 
                int Effortfulness 
                string Status ("Zaakceptowane", "W trakcie", "Zakończone", "Anulowane")!!
        */

    }
}
