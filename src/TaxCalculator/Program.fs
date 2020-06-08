open System
open TaxDomain

module CLIModule =
    [<EntryPoint>]
    let main argv =
        Console.Write("Enter your total gross income per year: ")
        let totalGrossIncome = Console.ReadLine() |> parseMoney
        match totalGrossIncome with
        | Error e ->
            Console.WriteLine e
            0
        | Ok totalGrossIncome ->
            Console.Write("Are you blind? (Y/N)")
            let blindAllowance =
                Console.ReadLine()
                |> getIsBlindAllowance 
            
            let marriageAllowance =
                Console.Write("Has your partner transfered marriage allowance to you? (Y/N)")
                let response = Console.ReadLine()
                getMarriageAllowance response totalGrossIncome
            
            Console.Write("Do you live in Scotland? (Y/N)")
            let livesInScotland = 
                Console.ReadLine()
                |> getLivesInScotland

            let allowances = [blindAllowance; marriageAllowance]
            let personalAllowance = getPersonalAllowance totalGrossIncome allowances
            Console.WriteLine("Personal allowance = " + personalAllowance.ToString()) 
            let taxRate =
                if livesInScotland then getScotlandTaxRate totalGrossIncome
                else getCommonTaxRate totalGrossIncome
            Console.WriteLine("Tax rate = " + taxRate.ToString())

            let taxableIncome = totalGrossIncome - personalAllowance
            let incomeTax = taxableIncome * taxRate
            Console.WriteLine("Your income tax is: " + incomeTax.ToString())
            0