open System
open TaxDomain

module CLIModule =
    let printInputError () =
        Console.WriteLine("Invalid input - please enter the character Y/y for yes, or N/n for no.")

    let inputBlindAllowance () =
        Console.Write("Are you blind? (Y/N)")
        let response = Console.ReadLine()
        getIsBlindAllowance response printInputError

    let inputMarriageAllowance totalGrossIncome =
        Console.Write("Has your partner transfered marriage allowance to you? (Y/N)")
        let response = Console.ReadLine()
        getMarriageAllowance response totalGrossIncome printInputError

    let inputLivesInScotland () =
        Console.Write("Do you live in Scotland? (Y/N)")
        let response = Console.ReadLine()
        getLivesInScotland response printInputError

    [<EntryPoint>]
    let main argv =
        Console.Write("Enter your total gross income per year: ")
        let totalGrossIncome = Console.ReadLine() |> parseMoney
        match totalGrossIncome with
        | Error e ->
            Console.WriteLine e
            0
        | Ok totalGrossIncome ->
            Console.WriteLine totalGrossIncome

            let allowances = [inputBlindAllowance(); inputMarriageAllowance totalGrossIncome]
            let personalAllowance = getPersonalAllowance totalGrossIncome allowances
            let livesInScotland = inputLivesInScotland()
            let taxRate =
                if livesInScotland then getScotlandTaxRate totalGrossIncome
                else getCommonTaxRate totalGrossIncome
            let taxableIncome = totalGrossIncome - personalAllowance
            let incomeTax = taxableIncome * taxRate
            Console.WriteLine("Your income tax is: " + incomeTax.ToString())
            0